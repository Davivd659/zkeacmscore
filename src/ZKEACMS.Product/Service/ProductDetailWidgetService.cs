/* http://www.zkea.net/ Copyright 2016 ZKEASOFT http://www.zkea.net/licenses */
using System;
using Easy;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ZKEACMS.Product.Models;
using ZKEACMS.Widget;

namespace ZKEACMS.Product.Service
{
    public class ProductDetailWidgetService : WidgetService<ProductDetailWidget,ProductDbContext>
    {
        private readonly IProductService _productService;
        public ProductDetailWidgetService(IWidgetBasePartService widgetService, IProductService productService, IApplicationContext applicationContext)
            : base(widgetService, applicationContext)
        {
            _productService = productService;
        }

        public override DbSet<ProductDetailWidget> CurrentDbSet
        {
            get
            {
                return DbContext.ProductDetailWidget;
            }
        }

        public override WidgetViewModelPart Display(WidgetBase widget, HttpContext httpContext)
        {
            int productId = 0;
            int.TryParse(httpContext.Request.Query["id"], out productId);
            var product = _productService.Get(productId) ?? new ProductEntity
            {
                Title = "产品明细组件使用说明",
                ImageUrl = "~/Modules/Product/Content/Image/Example.png",
                ProductContent = "<p>如上图所示，该组件需要一个<code>产品列表组件</code>组合使用，您需要在其它页面添加一个产品列表组件并链接过来，然后点击产品列表中的产品，该组件就可正常显示产品的内容</p>",
                CreatebyName = "ZKEASOFT"
            };

            var page = httpContext.GetLayout().Page;
            page.MetaDescription = product.SEODescription;
            page.MetaKeyWorlds = product.SEOKeyWord;
            page.Title = product.SEOTitle ?? product.Title;

            return widget.ToWidgetViewModelPart(product);
        }
    }
}