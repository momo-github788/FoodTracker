using System.Linq.Expressions;
using backend.Data;
using backend.Filter;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository.Impl {
    public class FoodRecordRepositoryImpl : RepositoryImpl<FoodRecord, string>, FoodRecordRepository {
        public FoodRecordRepositoryImpl(ApplicationDbContext context) :base(context){

        }
    }
}
