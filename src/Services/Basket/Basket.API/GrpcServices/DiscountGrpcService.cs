using System;
using System.Threading.Tasks;
using Discount.Grpc.Protos;

namespace Basket.API.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
        public DiscountGrpcService()
        {
            _discountProtoServiceClient = _discountProtoServiceClient ??
                                          throw new ArgumentNullException(nameof(_discountProtoServiceClient));
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var discountRequest = new GetDiscountRequest(){ProductName =  productName};
            return await _discountProtoServiceClient.GetDiscountAsync(discountRequest);
        }
    }
}
