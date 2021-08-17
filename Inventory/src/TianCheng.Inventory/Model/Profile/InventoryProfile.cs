using AutoMapper;
using TianCheng.Common;
using TianCheng.Inventory.DTO;

namespace TianCheng.Inventory.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class InventoryProfile : Profile, IAutoMapperProfile
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public InventoryProfile()
        {
            Register();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Register()
        {
            CreateMap<BrandInfo, BrandView>();
            CreateMap<BrandView, BrandInfo>(MemberList.None);

            CreateMap<CategoryInfo, CategoryView>();
            CreateMap<CategoryView, CategoryInfo>(MemberList.None);
            CreateMap<CategoryInfo, SelectView>(MemberList.None);

            CreateMap<GoodsInfo, GoodsView>();
            CreateMap<GoodsInfo, SelectView>(MemberList.None);
            CreateMap<GoodsInfo, GoodsSimpleView>();
            CreateMap<GoodsView, GoodsInfo>(MemberList.None);

            CreateMap<InventoryDetailInfo, InventoryDetailView>();
            CreateMap<InventoryDetailView, InventoryDetailInfo>(MemberList.None);

            CreateMap<GoodsSpecificationInfo, GoodsSpecificationView>();
            CreateMap<GoodsSpecificationView, GoodsSpecificationInfo>(MemberList.None);
        }
    }
}
