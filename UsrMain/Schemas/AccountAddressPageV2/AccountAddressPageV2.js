 define("AccountAddressPageV2", [], function() {
	return {
		entitySchemaName: "AccountAddress",
		methods: {
			/**
			 * @inheritDoc BasePageV2#asyncValidate
			 * @overridden
			 */
			asyncValidate: function(callback, scope) {
				this.callParent([function(response) {
					if (!this.validateResponse(response)) {
						return;
					}
					
					Terrasoft.chain(
						function(next) {
							this.validateDeliveryType(function(response) {
								if (this.validateResponse(response)) {
									next();
								}
							}, this);
						},
						function(next) {
							callback.call(scope, response);
							next();
						}, this);
					}, this]);
			},
			
			
			/**
			 * Валидирует поле "Тип адреса".
			 */
			validateDeliveryType: function(callback, scope) {
				let result = {
					success: true
				};
				
				let accountAddress = this.Ext.create("Terrasoft.EntitySchemaQuery", "AccountAddress");

				accountAddress.addColumn("Id", "Id");
				accountAddress.addColumn("Account", "Account");
				accountAddress.addColumn("AddressType", "AddressType");

				let accountFilter = accountAddress.createColumnFilterWithParameter(
					Terrasoft.ComparisonType.EQUAL,
					"Account",
					this.get("Account")
				);
				
				let addressTypeFilter = accountAddress.createColumnFilterWithParameter(
					Terrasoft.ComparisonType.EQUAL,
					"AddressType",
					this.get("AddressType")
				);

				accountAddress.filters.add("accountFilter", accountFilter);
				accountAddress.filters.add("addressTypeFilter", addressTypeFilter);

				accountAddress.getEntityCollection(function (response) {
					if (!response.success) {
						return;
					}
					
					if (response.collection.collection.length !== 0) {
						result.message = this.get("Resources.Strings.AlreadyHasDeliveryType");
						result.success = false;
					}
					
					callback.call(scope || this, result);
				}, this);
			},
		},
		diff: /**SCHEMA_DIFF*/ [] /**SCHEMA_DIFF*/,
	};
});
