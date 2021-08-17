﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TianCheng.Common;

namespace TianCheng.DAL.NpgByDapper
{
    /// <summary>
    /// PostgreSql数据库操作处理
    /// </summary>
    /// <typeparam name="PO">数据库存储对象</typeparam>
    /// <typeparam name="Q">查询对象</typeparam>
    /// <typeparam name="IdType">id类型</typeparam>
    public abstract partial class BaseOperation<PO, Q, IdType> : IDBOperationRegister,
        IDBOperation<PO>,
        IDBOperationFilter<PO, Q>
        where PO : IPrimaryKey<IdType>, new()
        where Q : QueryObject
    {
        #region Save
        /// <summary>
        /// 保存对象，根据ID是否为空来判断是新增还是修改操作
        /// </summary>
        /// <param name="entity"></param>
        public void Save(PO entity)
        {
            if (entity.IdEmpty)
            {
                Insert(entity);
            }
            else
            {
                Update(entity);
            }
        }
        #endregion

        #region 数据插入
        /// <summary>
        /// 初始化默认的插入sql
        /// returning id 是PostgreSql的方言，可以直接返回创建的id
        /// </summary>
        /// <returns></returns>
        protected virtual void InitDefaultInsertSQL()
        {
            IEnumerable<string> pn = PropertyList.Select(e => e.Name);
            DefaultInsertSQL = $"insert into {TableName} ({string.Join(',', pn.Select(e => $"\"{e}\""))}) values (@{string.Join(",@", pn)}) returning \"Id\";";
            DefaultInsertNotIdSQL = $"insert into {TableName} ({string.Join(',', pn.Where(e => e != "Id").Select(e => $"\"{e}\""))}) values (@{string.Join(",@", pn.Where(e => e != "Id"))}) returning \"Id\";";
        }
        /// <summary>
        /// 添加新记录
        /// </summary>
        /// <param name="obj"></param>
        public void InsertObject(PO obj)
        {
            Insert(obj);
        }
        /// <summary>
        /// 添加新记录
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public void Insert(PO obj, IDbTransaction tran = null, int? commandTimeout = null)
        {
            // 获取数据库连接
            IDbConnection connection = tran != null ? tran.Connection : GetConnection();
            // 获取执行的sql
            string sql = obj.IdEmpty ? DefaultInsertNotIdSQL : DefaultInsertSQL;

            // 打开数据库连接
            ConnectionOpen(connection, tran);

            try
            {
                // 执行插入命令
                obj.Id = connection.ExecuteScalar<IdType>(sql, obj, tran, commandTimeout);
            }
            catch (Exception ex)
            {
                throw new NpgOperationException(ex, $"Insert数据异常  \r\nsql：{sql}   \r\n插入对象：{obj.ToJson()}");
            }

            // 关闭数据库连接
            ConnectionClose(connection, tran);
        }

        /// <summary>
        /// 批量添加新记录
        /// </summary>
        /// <param name="obj"></param>
        public void InsertRange(IEnumerable<PO> data)
        {
            Insert(data);
        }
        /// <summary>
        /// 批量添加新记录
        /// </summary>
        /// <param name="data"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public void Insert(IEnumerable<PO> data, IDbTransaction tran = null, int? commandTimeout = null)
        {
            // 组织sql与参数
            var param = new DynamicParameters();
            string[] itemArray = new string[data.Count()];
            bool idEmpty = AnyIdEmpty(data);
            for (int index = 1; index <= data.Count(); index++)
            {
                itemArray[index - 1] = ParamName(idEmpty, index);
                SetParamValue(idEmpty, index, param, data.ElementAt(index - 1));
            }
            string sql = $"insert into {TableName} ({FieldList(idEmpty)}) values {string.Join(",", itemArray)} returning id;";

            // 获取数据库连接
            IDbConnection connection = tran != null ? tran.Connection : GetConnection();
            // 打开数据库连接
            ConnectionOpen(connection, tran);
            try
            {
                // 执行多数据保存，返回id列表
                IDataReader reader = connection.ExecuteReader(sql, param, tran, commandTimeout);
                // 将返回id添加到对应对象列表中
                int ri = 0;
                while (reader.Read())
                {
                    data.ElementAt(ri++).SetId(reader.GetValue(0).ToString());
                }
            }
            catch (Exception ex)
            {
                throw new NpgOperationException(ex, $"Insert数据集合异常 \r\nsql：{sql}   \r\n插入对象：{data.ToJson()}");
            }
            // 关闭数据库连接
            ConnectionClose(connection, tran);
        }

        /// <summary>
        /// 列表数据有1个为空id则认为，均为空id
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool AnyIdEmpty(IEnumerable<PO> data)
        {
            foreach (var info in data)
            {
                if (info.IdEmpty) return true;
            }
            return false;
        }
        /// <summary>
        /// 获取sql中字段名称
        /// </summary>
        /// <param name="idEmpty"></param>
        /// <returns></returns>
        private string FieldList(bool idEmpty)
        {
            if (idEmpty)
            {
                return string.Join(',', PropertyList.Where(e => e.Name != "id").Select(e => e.Name));
            }
            else
            {
                return string.Join(',', PropertyList.Select(e => e.Name));
            }
        }
        /// <summary>
        /// 获取sql中参数名称
        /// </summary>
        /// <param name="idEmpty"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string ParamName(bool idEmpty, int index)
        {
            if (idEmpty)
            {
                return $"(@{ string.Join($"{index},@", PropertyList.Where(e => e.Name != "id").Select(e => e.Name))}{index})";
            }
            else
            {
                return $"(@{ string.Join($"{index},@", PropertyList.Select(e => e.Name))}{index})";
            }
        }
        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="idEmpty"></param>
        /// <param name="index"></param>
        /// <param name="param"></param>
        /// <param name="info"></param>
        private void SetParamValue(bool idEmpty, int index, DynamicParameters param, PO info)
        {
            foreach (var p in PropertyList)
            {
                if (idEmpty && p.Name.Equals("id")) { continue; }

                param.Add($"{p.Name}{index}", p.GetValue(info));
            }
        }
        #endregion

        #region 更新
        /// <summary>
        /// 初始化更新语句
        /// </summary>
        protected virtual void InitDefaultUpdateSQL()
        {
            DefaultUpdateSQL = $"update {TableName} set {string.Join(",", PropertyList.Select(e => e.Name).Where(e => e != "id").Select(e => $"{e}=@{e}").ToArray())} where id=@id";
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="obj"></param>
        public bool UpdateObject(PO obj)
        {
            return Update(obj);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        public bool Update(PO obj, IDbTransaction tran = null, int? commandTimeout = null)
        {
            // 获取数据库连接
            IDbConnection connection = tran != null ? tran.Connection : GetConnection();

            DynamicParameters param = new DynamicParameters();
            foreach (var p in PropertyList)
            {
                param.Add($"{p.Name}", p.GetValue(obj));
            }

            // 打开数据库连接
            ConnectionOpen(connection, tran);
            try
            {
                // 执行更新命令
                connection.ExecuteScalar<IdType>(DefaultUpdateSQL, param, tran, commandTimeout);
            }
            catch (Exception ex)
            {
                throw new NpgOperationException(ex, $"Update数据异常  \r\nsql：{DefaultUpdateSQL}   \r\n更新对象：{param.ToJson()}");
            }
            // 关闭数据库连接
            ConnectionClose(connection, tran);
            return true;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="updateSql"></param>
        /// <param name="parameters"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns>返回更新的行数</returns>
        public int Update(string updateSql, DynamicParameters parameters = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            // 获取数据库连接
            IDbConnection connection = tran != null ? tran.Connection : GetConnection();

            // 打开数据库连接
            ConnectionOpen(connection, tran);
            int result = 0;
            try
            {
                // 执行更新命令
                object result1 = connection.ExecuteScalar(updateSql, parameters, tran, commandTimeout);
            }
            catch (Exception ex)
            {
                throw new NpgOperationException(ex, $"sql命令更新数据异常  \r\nsql：{connection}   \r\n更新参数：{parameters.ToJson()}");
            }

            // 关闭数据库连接
            ConnectionClose(connection, tran);
            return result;
        }

        /// <summary>
        /// 更新一组数据，，有待完善
        /// </summary>
        /// <param name="entities"></param>
        public void UpdateRange(IEnumerable<PO> entities)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
