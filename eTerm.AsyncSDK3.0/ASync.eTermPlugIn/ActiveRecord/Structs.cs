


using System;
using SubSonic.Schema;
using System.Collections.Generic;
using SubSonic.DataProviders;
using System.Data;

namespace ASync.eTermPlugIn {
	
        /// <summary>
        /// Table: ASync_BaseFare
        /// Primary Key: DepartureCity
        /// </summary>

        public class ASync_BaseFareTable: DatabaseTable {
            
            public ASync_BaseFareTable(IDataProvider provider):base("ASync_BaseFare",provider){
                ClassName = "ASync_BaseFare";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("DepartureCity", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 3
                });

                Columns.Add(new DatabaseColumn("DestinationCity", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 3
                });

                Columns.Add(new DatabaseColumn("Distance", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Double,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("BaseFare", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Double,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn DepartureCity{
                get{
                    return this.GetColumn("DepartureCity");
                }
            }
				
   			public static string DepartureCityColumn{
			      get{
        			return "DepartureCity";
      			}
		    }
            
            public IColumn DestinationCity{
                get{
                    return this.GetColumn("DestinationCity");
                }
            }
				
   			public static string DestinationCityColumn{
			      get{
        			return "DestinationCity";
      			}
		    }
            
            public IColumn Distance{
                get{
                    return this.GetColumn("Distance");
                }
            }
				
   			public static string DistanceColumn{
			      get{
        			return "Distance";
      			}
		    }
            
            public IColumn BaseFare{
                get{
                    return this.GetColumn("BaseFare");
                }
            }
				
   			public static string BaseFareColumn{
			      get{
        			return "BaseFare";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ASync_RealityFare
        /// Primary Key: AirCompany
        /// </summary>

        public class ASync_RealityFareTable: DatabaseTable {
            
            public ASync_RealityFareTable(IDataProvider provider):base("ASync_RealityFare",provider){
                ClassName = "ASync_RealityFare";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("DepartureCity", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 3
                });

                Columns.Add(new DatabaseColumn("DestinationCity", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 3
                });

                Columns.Add(new DatabaseColumn("AirCompany", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 2
                });

                Columns.Add(new DatabaseColumn("CabinCode", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 2
                });

                Columns.Add(new DatabaseColumn("CabinDiscount", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Double,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SinglePrice", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Double,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("RoundPrice", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Double,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn DepartureCity{
                get{
                    return this.GetColumn("DepartureCity");
                }
            }
				
   			public static string DepartureCityColumn{
			      get{
        			return "DepartureCity";
      			}
		    }
            
            public IColumn DestinationCity{
                get{
                    return this.GetColumn("DestinationCity");
                }
            }
				
   			public static string DestinationCityColumn{
			      get{
        			return "DestinationCity";
      			}
		    }
            
            public IColumn AirCompany{
                get{
                    return this.GetColumn("AirCompany");
                }
            }
				
   			public static string AirCompanyColumn{
			      get{
        			return "AirCompany";
      			}
		    }
            
            public IColumn CabinCode{
                get{
                    return this.GetColumn("CabinCode");
                }
            }
				
   			public static string CabinCodeColumn{
			      get{
        			return "CabinCode";
      			}
		    }
            
            public IColumn CabinDiscount{
                get{
                    return this.GetColumn("CabinDiscount");
                }
            }
				
   			public static string CabinDiscountColumn{
			      get{
        			return "CabinDiscount";
      			}
		    }
            
            public IColumn SinglePrice{
                get{
                    return this.GetColumn("SinglePrice");
                }
            }
				
   			public static string SinglePriceColumn{
			      get{
        			return "SinglePrice";
      			}
		    }
            
            public IColumn RoundPrice{
                get{
                    return this.GetColumn("RoundPrice");
                }
            }
				
   			public static string RoundPriceColumn{
			      get{
        			return "RoundPrice";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ASync_CityCode
        /// Primary Key: CityCode
        /// </summary>

        public class ASync_CityCodeTable: DatabaseTable {
            
            public ASync_CityCodeTable(IDataProvider provider):base("ASync_CityCode",provider){
                ClassName = "ASync_CityCode";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("CityId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 10
                });

                Columns.Add(new DatabaseColumn("CityCode", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 5
                });

                Columns.Add(new DatabaseColumn("CityName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 50
                });

                Columns.Add(new DatabaseColumn("CityPinYin", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 50
                });

                Columns.Add(new DatabaseColumn("CountryCode", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 10
                });

                Columns.Add(new DatabaseColumn("Flag", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ParentId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 10
                });
                    
                
                
            }

            public IColumn CityId{
                get{
                    return this.GetColumn("CityId");
                }
            }
				
   			public static string CityIdColumn{
			      get{
        			return "CityId";
      			}
		    }
            
            public IColumn CityCode{
                get{
                    return this.GetColumn("CityCode");
                }
            }
				
   			public static string CityCodeColumn{
			      get{
        			return "CityCode";
      			}
		    }
            
            public IColumn CityName{
                get{
                    return this.GetColumn("CityName");
                }
            }
				
   			public static string CityNameColumn{
			      get{
        			return "CityName";
      			}
		    }
            
            public IColumn CityPinYin{
                get{
                    return this.GetColumn("CityPinYin");
                }
            }
				
   			public static string CityPinYinColumn{
			      get{
        			return "CityPinYin";
      			}
		    }
            
            public IColumn CountryCode{
                get{
                    return this.GetColumn("CountryCode");
                }
            }
				
   			public static string CountryCodeColumn{
			      get{
        			return "CountryCode";
      			}
		    }
            
            public IColumn Flag{
                get{
                    return this.GetColumn("Flag");
                }
            }
				
   			public static string FlagColumn{
			      get{
        			return "Flag";
      			}
		    }
            
            public IColumn ParentId{
                get{
                    return this.GetColumn("ParentId");
                }
            }
				
   			public static string ParentIdColumn{
			      get{
        			return "ParentId";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Async_Log
        /// Primary Key: Id
        /// </summary>

        public class Async_LogTable: DatabaseTable {
            
            public Async_LogTable(IDataProvider provider):base("Async_Log",provider){
                ClassName = "Async_Log";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int64,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("eTermSession", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 20
                });

                Columns.Add(new DatabaseColumn("ClientSession", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 20
                });

                Columns.Add(new DatabaseColumn("ASynCommand", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 250
                });

                Columns.Add(new DatabaseColumn("ASyncResult", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 1500
                });

                Columns.Add(new DatabaseColumn("LogDate", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn eTermSession{
                get{
                    return this.GetColumn("eTermSession");
                }
            }
				
   			public static string eTermSessionColumn{
			      get{
        			return "eTermSession";
      			}
		    }
            
            public IColumn ClientSession{
                get{
                    return this.GetColumn("ClientSession");
                }
            }
				
   			public static string ClientSessionColumn{
			      get{
        			return "ClientSession";
      			}
		    }
            
            public IColumn ASynCommand{
                get{
                    return this.GetColumn("ASynCommand");
                }
            }
				
   			public static string ASynCommandColumn{
			      get{
        			return "ASynCommand";
      			}
		    }
            
            public IColumn ASyncResult{
                get{
                    return this.GetColumn("ASyncResult");
                }
            }
				
   			public static string ASyncResultColumn{
			      get{
        			return "ASyncResult";
      			}
		    }
            
            public IColumn LogDate{
                get{
                    return this.GetColumn("LogDate");
                }
            }
				
   			public static string LogDateColumn{
			      get{
        			return "LogDate";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Async_PNR
        /// Primary Key: IdentityId
        /// </summary>

        public class Async_PNRTable: DatabaseTable {
            
            public Async_PNRTable(IDataProvider provider):base("Async_PNR",provider){
                ClassName = "Async_PNR";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("IdentityId", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int64,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ClientSession", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 25
                });

                Columns.Add(new DatabaseColumn("PnrCode", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 15
                });

                Columns.Add(new DatabaseColumn("SourcePnr", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("UpdateDate", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn IdentityId{
                get{
                    return this.GetColumn("IdentityId");
                }
            }
				
   			public static string IdentityIdColumn{
			      get{
        			return "IdentityId";
      			}
		    }
            
            public IColumn ClientSession{
                get{
                    return this.GetColumn("ClientSession");
                }
            }
				
   			public static string ClientSessionColumn{
			      get{
        			return "ClientSession";
      			}
		    }
            
            public IColumn PnrCode{
                get{
                    return this.GetColumn("PnrCode");
                }
            }
				
   			public static string PnrCodeColumn{
			      get{
        			return "PnrCode";
      			}
		    }
            
            public IColumn SourcePnr{
                get{
                    return this.GetColumn("SourcePnr");
                }
            }
				
   			public static string SourcePnrColumn{
			      get{
        			return "SourcePnr";
      			}
		    }
            
            public IColumn UpdateDate{
                get{
                    return this.GetColumn("UpdateDate");
                }
            }
				
   			public static string UpdateDateColumn{
			      get{
        			return "UpdateDate";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ASync_AirLine
        /// Primary Key: AirLineCode
        /// </summary>

        public class ASync_AirLineTable: DatabaseTable {
            
            public ASync_AirLineTable(IDataProvider provider):base("ASync_AirLine",provider){
                ClassName = "ASync_AirLine";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("AirLineCode", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 5
                });

                Columns.Add(new DatabaseColumn("ChineseName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 50
                });

                Columns.Add(new DatabaseColumn("EnglishName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 50
                });
                    
                
                
            }

            public IColumn AirLineCode{
                get{
                    return this.GetColumn("AirLineCode");
                }
            }
				
   			public static string AirLineCodeColumn{
			      get{
        			return "AirLineCode";
      			}
		    }
            
            public IColumn ChineseName{
                get{
                    return this.GetColumn("ChineseName");
                }
            }
				
   			public static string ChineseNameColumn{
			      get{
        			return "ChineseName";
      			}
		    }
            
            public IColumn EnglishName{
                get{
                    return this.GetColumn("EnglishName");
                }
            }
				
   			public static string EnglishNameColumn{
			      get{
        			return "EnglishName";
      			}
		    }
            
                    
        }
        
}