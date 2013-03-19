using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DAF.Core.Data;
using DAF.Core.Search;
using DAF.Core.Search.Lucene;

namespace SqlDataLuceneChange
{
    public class UserChangeHandler : LuceneDataChangeHandler<User>
    {
        public UserChangeHandler(string connStringName, IRepository<User> repoUser, ISearchProvider searchProvider)
            : base(connStringName, repoUser, searchProvider)
        {
        }

        protected override Expression<Func<User, bool>> GetKeyPrediction(string key)
        {
            return o => o.UserId == key;
        }

        public override string TableName
        {
            get { return "sso_User"; }
        }
    }
}
