 namespace Terrasoft.Configuration.UsrAccountService
{
    using System;
	using System.Collections.Generic;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.ServiceModel.Activation;
    using Terrasoft.Core;
	using Terrasoft.Core.DB;
    using Terrasoft.Web.Common;
    using Terrasoft.Core.Entities; 

    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class UsrAccountService: BaseService
    {
        
        /// <summary>
		/// Находит количество Контрагентов, содержащих строку в названии, переданную параметром. 
		/// </summary>
		/// <param name="substring">Строка в названии.</param>
		/// <returns>Количество Контрагентов.</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public int GetCountAccountWithSubstring(string substring) {	
			var select = new Select(UserConnection)
					.Column(Func.Count("Name")).As("Count")
				.From("Account")
				.Where("Name")
					.IsLike(Column.Parameter($"%{substring}%"))
				as Select;
				
			return select.ExecuteScalar<int>();
        }
    }
}