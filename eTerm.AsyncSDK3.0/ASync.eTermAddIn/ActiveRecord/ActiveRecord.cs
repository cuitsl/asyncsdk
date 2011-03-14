


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SubSonic.DataProviders;
using SubSonic.Extensions;
using System.Linq.Expressions;
using SubSonic.Schema;
using System.Collections;
using SubSonic;
using SubSonic.Repository;
using System.ComponentModel;
using System.Data.Common;

namespace ASync.eTermAddIn
{
    
    
    /// <summary>
    /// A class which represents the ASync_BaseFare table in the ASync Database.
    /// </summary>
    public partial class ASync_BaseFare: IActiveRecord
    {
    
        #region Built-in testing
        static TestRepository<ASync_BaseFare> _testRepo;
        

        
        static void SetTestRepo(){
            _testRepo = _testRepo ?? new TestRepository<ASync_BaseFare>(new ASync.eTermAddIn.ASyncDB());
        }
        public static void ResetTestRepo(){
            _testRepo = null;
            SetTestRepo();
        }
        public static void Setup(List<ASync_BaseFare> testlist){
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }
        public static void Setup(ASync_BaseFare item) {
            SetTestRepo();
            _testRepo._items.Add(item);
        }
        public static void Setup(int testItems) {
            SetTestRepo();
            for(int i=0;i<testItems;i++){
                ASync_BaseFare item=new ASync_BaseFare();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;


        #endregion

        IRepository<ASync_BaseFare> _repo;
        ITable tbl;
        bool _isNew;
        public bool IsNew(){
            return _isNew;
        }
        
        public void SetIsLoaded(bool isLoaded){
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        public void SetIsNew(bool isNew){
            _isNew=isNew;
        }
        bool _isLoaded;
        public bool IsLoaded(){
            return _isLoaded;
        }
                
        List<IColumn> _dirtyColumns;
        public bool IsDirty(){
            return _dirtyColumns.Count>0;
        }
        
        public List<IColumn> GetDirtyColumns (){
            return _dirtyColumns;
        }

        ASync.eTermAddIn.ASyncDB _db;
        public ASync_BaseFare(string connectionString, string providerName) {

            _db=new ASync.eTermAddIn.ASyncDB(connectionString, providerName);
            Init();            
         }
         
        public ASync_BaseFare(string connectionString)
            : this(connectionString, @"System.Data.SqlClient") { 
            
        }
         
        void Init(){
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode){
                ASync_BaseFare.SetTestRepo();
                _repo=_testRepo;
            }else{
                _repo = new SubSonicRepository<ASync_BaseFare>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
            OnCreated();       

        }
        
        public ASync_BaseFare(){
             _db=new ASync.eTermAddIn.ASyncDB();
            Init();            
        }
        
       
        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaved();
        
        partial void OnChanged();
        
        public IList<IColumn> Columns{
            get{
                return tbl.Columns;
            }
        }

        public ASync_BaseFare(Expression<Func<ASync_BaseFare, bool>> expression):this() {

            SetIsLoaded(_repo.Load(this,expression));
        }
        
       
        
        internal static IRepository<ASync_BaseFare> GetRepo(string connectionString, string providerName){
            ASync.eTermAddIn.ASyncDB db;
            if(String.IsNullOrEmpty(connectionString)){
                db=new ASync.eTermAddIn.ASyncDB();
            }else{
                db=new ASync.eTermAddIn.ASyncDB(connectionString, providerName);
            }
            IRepository<ASync_BaseFare> _repo;
            
            if(db.TestMode){
                ASync_BaseFare.SetTestRepo();
                _repo=_testRepo;
            }else{
                _repo = new SubSonicRepository<ASync_BaseFare>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<ASync_BaseFare> GetRepo(){
            return GetRepo("","");
        }
        
        public static ASync_BaseFare SingleOrDefault(Expression<Func<ASync_BaseFare, bool>> expression) {

            var repo = GetRepo();
            var results=repo.Find(expression);
            ASync_BaseFare single=null;
            if(results.Count() > 0){
                single=results.ToList()[0];
                single.OnLoaded();
                single.SetIsLoaded(true);
                single.SetIsNew(false);
            }

            return single;
        }      
        
        public static ASync_BaseFare SingleOrDefault(Expression<Func<ASync_BaseFare, bool>> expression,string connectionString, string providerName) {
            var repo = GetRepo(connectionString,providerName);
            var results=repo.Find(expression);
            ASync_BaseFare single=null;
            if(results.Count() > 0){
                single=results.ToList()[0];
            }

            return single;


        }
        
        
        public static bool Exists(Expression<Func<ASync_BaseFare, bool>> expression,string connectionString, string providerName) {
           
            return All(connectionString,providerName).Any(expression);
        }        
        public static bool Exists(Expression<Func<ASync_BaseFare, bool>> expression) {
           
            return All().Any(expression);
        }        

        public static IList<ASync_BaseFare> Find(Expression<Func<ASync_BaseFare, bool>> expression) {
            
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<ASync_BaseFare> Find(Expression<Func<ASync_BaseFare, bool>> expression,string connectionString, string providerName) {

            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();

        }
        public static IQueryable<ASync_BaseFare> All(string connectionString, string providerName) {
            return GetRepo(connectionString,providerName).GetAll();
        }
        public static IQueryable<ASync_BaseFare> All() {
            return GetRepo().GetAll();
        }
        
        public static PagedList<ASync_BaseFare> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<ASync_BaseFare> GetPaged(string sortBy, int pageIndex, int pageSize) {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<ASync_BaseFare> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
            
        }


        public static PagedList<ASync_BaseFare> GetPaged(int pageIndex, int pageSize) {
            return GetRepo().GetPaged(pageIndex, pageSize);
            
        }

        public string KeyName()
        {
            return "DepartureCity";
        }

        public object KeyValue()
        {
            return this.DepartureCity;
        }
        
        public void SetKeyValue(object value) {
            if (value != null && value!=DBNull.Value) {
                var settable = value.ChangeTypeTo<string>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
        public override string ToString(){
                            return this.DepartureCity.ToString();
                    }

        public override bool Equals(object obj){
            if(obj.GetType()==typeof(ASync_BaseFare)){
                ASync_BaseFare compare=(ASync_BaseFare)obj;
                return compare.KeyValue()==this.KeyValue();
            }else{
                return base.Equals(obj);
            }
        }

        public string DescriptorValue()
        {
                            return this.DepartureCity.ToString();
                    }

        public string DescriptorColumn() {
            return "DepartureCity";
        }
        public static string GetKeyColumn()
        {
            return "DepartureCity";
        }        
        public static string GetDescriptorColumn()
        {
            return "DepartureCity";
        }
        
        #region ' Foreign Keys '
        public IQueryable<ASync_RealityFare> ASync_RealityFares
        {
            get
            {
                
                  var repo=ASync.eTermAddIn.ASync_RealityFare.GetRepo();
                  return from items in repo.GetAll()
                       where items.DepartureCity == _DepartureCity
                       select items;
            }
        }

        public IQueryable<ASync_RealityFare> ASync_RealityFares1
        {
            get
            {
                
                  var repo=ASync.eTermAddIn.ASync_RealityFare.GetRepo();
                  return from items in repo.GetAll()
                       where items.DestinationCity == _DepartureCity
                       select items;
            }
        }

        public IQueryable<ASync_RealityFare> ASync_RealityFares2
        {
            get
            {
                
                  var repo=ASync.eTermAddIn.ASync_RealityFare.GetRepo();
                  return from items in repo.GetAll()
                       where items.DepartureCity == _DestinationCity
                       select items;
            }
        }

        public IQueryable<ASync_RealityFare> ASync_RealityFares3
        {
            get
            {
                
                  var repo=ASync.eTermAddIn.ASync_RealityFare.GetRepo();
                  return from items in repo.GetAll()
                       where items.DestinationCity == _DestinationCity
                       select items;
            }
        }

        #endregion
        

        string _DepartureCity;
        public string DepartureCity
        {
            get { return _DepartureCity; }
            set
            {
                if(_DepartureCity!=value){
                    _DepartureCity=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="DepartureCity");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        string _DestinationCity;
        public string DestinationCity
        {
            get { return _DestinationCity; }
            set
            {
                if(_DestinationCity!=value){
                    _DestinationCity=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="DestinationCity");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        double? _Distance;
        public double? Distance
        {
            get { return _Distance; }
            set
            {
                if(_Distance!=value){
                    _Distance=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Distance");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        double? _BaseFare;
        public double? BaseFare
        {
            get { return _BaseFare; }
            set
            {
                if(_BaseFare!=value){
                    _BaseFare=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="BaseFare");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }



        public DbCommand GetUpdateCommand() {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }
        public DbCommand GetInsertCommand() {
 
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        
        public void Update(){
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider){
        
            
            if(this._dirtyColumns.Count>0){
                _repo.Update(this,provider);
                _dirtyColumns.Clear();    
            }
            OnSaved();
       }
 
        public void Add(){
            Add(_db.DataProvider);
        }
        
        
       
        public void Add(IDataProvider provider){

            
            var key=KeyValue();
            if(key==null){
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }else{
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
                
        
        public void Save() {
            Save(_db.DataProvider);
        }      
        public void Save(IDataProvider provider) {
            
           
            if (_isNew) {
                Add(provider);
                
            } else {
                Update(provider);
            }
            
        }

        

        public void Delete(IDataProvider provider) {
                   
                 
            _repo.Delete(KeyValue());
            
                    }


        public void Delete() {
            Delete(_db.DataProvider);
        }


        public static void Delete(Expression<Func<ASync_BaseFare, bool>> expression) {
            var repo = GetRepo();
            
       
            
            repo.DeleteMany(expression);
            
        }

        

        public void Load(IDataReader rdr) {
            Load(rdr, true);
        }
        public void Load(IDataReader rdr, bool closeReader) {
            if (rdr.Read()) {

                try {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                } catch {
                    SetIsLoaded(false);
                    throw;
                }
            }else{
                SetIsLoaded(false);
            }

            if (closeReader)
                rdr.Dispose();
        }
        

    } 
    
    
    /// <summary>
    /// A class which represents the ASync_RealityFare table in the ASync Database.
    /// </summary>
    public partial class ASync_RealityFare: IActiveRecord
    {
    
        #region Built-in testing
        static TestRepository<ASync_RealityFare> _testRepo;
        

        
        static void SetTestRepo(){
            _testRepo = _testRepo ?? new TestRepository<ASync_RealityFare>(new ASync.eTermAddIn.ASyncDB());
        }
        public static void ResetTestRepo(){
            _testRepo = null;
            SetTestRepo();
        }
        public static void Setup(List<ASync_RealityFare> testlist){
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }
        public static void Setup(ASync_RealityFare item) {
            SetTestRepo();
            _testRepo._items.Add(item);
        }
        public static void Setup(int testItems) {
            SetTestRepo();
            for(int i=0;i<testItems;i++){
                ASync_RealityFare item=new ASync_RealityFare();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;


        #endregion

        IRepository<ASync_RealityFare> _repo;
        ITable tbl;
        bool _isNew;
        public bool IsNew(){
            return _isNew;
        }
        
        public void SetIsLoaded(bool isLoaded){
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        public void SetIsNew(bool isNew){
            _isNew=isNew;
        }
        bool _isLoaded;
        public bool IsLoaded(){
            return _isLoaded;
        }
                
        List<IColumn> _dirtyColumns;
        public bool IsDirty(){
            return _dirtyColumns.Count>0;
        }
        
        public List<IColumn> GetDirtyColumns (){
            return _dirtyColumns;
        }

        ASync.eTermAddIn.ASyncDB _db;
        public ASync_RealityFare(string connectionString, string providerName) {

            _db=new ASync.eTermAddIn.ASyncDB(connectionString, providerName);
            Init();            
         }
         
        public ASync_RealityFare(string connectionString)
            : this(connectionString, @"System.Data.SqlClient") { 
            
        }
         
        void Init(){
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode){
                ASync_RealityFare.SetTestRepo();
                _repo=_testRepo;
            }else{
                _repo = new SubSonicRepository<ASync_RealityFare>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
            OnCreated();       

        }
        
        public ASync_RealityFare(){
             _db=new ASync.eTermAddIn.ASyncDB();
            Init();            
        }
        
       
        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaved();
        
        partial void OnChanged();
        
        public IList<IColumn> Columns{
            get{
                return tbl.Columns;
            }
        }

        public ASync_RealityFare(Expression<Func<ASync_RealityFare, bool>> expression):this() {

            SetIsLoaded(_repo.Load(this,expression));
        }
        
       
        
        internal static IRepository<ASync_RealityFare> GetRepo(string connectionString, string providerName){
            ASync.eTermAddIn.ASyncDB db;
            if(String.IsNullOrEmpty(connectionString)){
                db=new ASync.eTermAddIn.ASyncDB();
            }else{
                db=new ASync.eTermAddIn.ASyncDB(connectionString, providerName);
            }
            IRepository<ASync_RealityFare> _repo;
            
            if(db.TestMode){
                ASync_RealityFare.SetTestRepo();
                _repo=_testRepo;
            }else{
                _repo = new SubSonicRepository<ASync_RealityFare>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<ASync_RealityFare> GetRepo(){
            return GetRepo("","");
        }
        
        public static ASync_RealityFare SingleOrDefault(Expression<Func<ASync_RealityFare, bool>> expression) {

            var repo = GetRepo();
            var results=repo.Find(expression);
            ASync_RealityFare single=null;
            if(results.Count() > 0){
                single=results.ToList()[0];
                single.OnLoaded();
                single.SetIsLoaded(true);
                single.SetIsNew(false);
            }

            return single;
        }      
        
        public static ASync_RealityFare SingleOrDefault(Expression<Func<ASync_RealityFare, bool>> expression,string connectionString, string providerName) {
            var repo = GetRepo(connectionString,providerName);
            var results=repo.Find(expression);
            ASync_RealityFare single=null;
            if(results.Count() > 0){
                single=results.ToList()[0];
            }

            return single;


        }
        
        
        public static bool Exists(Expression<Func<ASync_RealityFare, bool>> expression,string connectionString, string providerName) {
           
            return All(connectionString,providerName).Any(expression);
        }        
        public static bool Exists(Expression<Func<ASync_RealityFare, bool>> expression) {
           
            return All().Any(expression);
        }        

        public static IList<ASync_RealityFare> Find(Expression<Func<ASync_RealityFare, bool>> expression) {
            
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<ASync_RealityFare> Find(Expression<Func<ASync_RealityFare, bool>> expression,string connectionString, string providerName) {

            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();

        }
        public static IQueryable<ASync_RealityFare> All(string connectionString, string providerName) {
            return GetRepo(connectionString,providerName).GetAll();
        }
        public static IQueryable<ASync_RealityFare> All() {
            return GetRepo().GetAll();
        }
        
        public static PagedList<ASync_RealityFare> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<ASync_RealityFare> GetPaged(string sortBy, int pageIndex, int pageSize) {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<ASync_RealityFare> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
            
        }


        public static PagedList<ASync_RealityFare> GetPaged(int pageIndex, int pageSize) {
            return GetRepo().GetPaged(pageIndex, pageSize);
            
        }

        public string KeyName()
        {
            return "AirCompany";
        }

        public object KeyValue()
        {
            return this.AirCompany;
        }
        
        public void SetKeyValue(object value) {
            if (value != null && value!=DBNull.Value) {
                var settable = value.ChangeTypeTo<string>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
        public override string ToString(){
                            return this.DepartureCity.ToString();
                    }

        public override bool Equals(object obj){
            if(obj.GetType()==typeof(ASync_RealityFare)){
                ASync_RealityFare compare=(ASync_RealityFare)obj;
                return compare.KeyValue()==this.KeyValue();
            }else{
                return base.Equals(obj);
            }
        }

        public string DescriptorValue()
        {
                            return this.DepartureCity.ToString();
                    }

        public string DescriptorColumn() {
            return "DepartureCity";
        }
        public static string GetKeyColumn()
        {
            return "AirCompany";
        }        
        public static string GetDescriptorColumn()
        {
            return "DepartureCity";
        }
        
        #region ' Foreign Keys '
        public IQueryable<ASync_BaseFare> ASync_BaseFares
        {
            get
            {
                
                  var repo=ASync.eTermAddIn.ASync_BaseFare.GetRepo();
                  return from items in repo.GetAll()
                       where items.DepartureCity == _DepartureCity
                       select items;
            }
        }

        public IQueryable<ASync_BaseFare> ASync_BaseFares1
        {
            get
            {
                
                  var repo=ASync.eTermAddIn.ASync_BaseFare.GetRepo();
                  return from items in repo.GetAll()
                       where items.DepartureCity == _DestinationCity
                       select items;
            }
        }

        public IQueryable<ASync_BaseFare> ASync_BaseFares2
        {
            get
            {
                
                  var repo=ASync.eTermAddIn.ASync_BaseFare.GetRepo();
                  return from items in repo.GetAll()
                       where items.DestinationCity == _DepartureCity
                       select items;
            }
        }

        public IQueryable<ASync_BaseFare> ASync_BaseFares3
        {
            get
            {
                
                  var repo=ASync.eTermAddIn.ASync_BaseFare.GetRepo();
                  return from items in repo.GetAll()
                       where items.DestinationCity == _DestinationCity
                       select items;
            }
        }

        #endregion
        

        string _DepartureCity;
        public string DepartureCity
        {
            get { return _DepartureCity; }
            set
            {
                if(_DepartureCity!=value){
                    _DepartureCity=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="DepartureCity");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        string _DestinationCity;
        public string DestinationCity
        {
            get { return _DestinationCity; }
            set
            {
                if(_DestinationCity!=value){
                    _DestinationCity=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="DestinationCity");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        string _AirCompany;
        public string AirCompany
        {
            get { return _AirCompany; }
            set
            {
                if(_AirCompany!=value){
                    _AirCompany=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="AirCompany");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        string _CabinCode;
        public string CabinCode
        {
            get { return _CabinCode; }
            set
            {
                if(_CabinCode!=value){
                    _CabinCode=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CabinCode");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        double? _CabinDiscount;
        public double? CabinDiscount
        {
            get { return _CabinDiscount; }
            set
            {
                if(_CabinDiscount!=value){
                    _CabinDiscount=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CabinDiscount");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        double? _SinglePrice;
        public double? SinglePrice
        {
            get { return _SinglePrice; }
            set
            {
                if(_SinglePrice!=value){
                    _SinglePrice=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="SinglePrice");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        double? _RoundPrice;
        public double? RoundPrice
        {
            get { return _RoundPrice; }
            set
            {
                if(_RoundPrice!=value){
                    _RoundPrice=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="RoundPrice");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }



        public DbCommand GetUpdateCommand() {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }
        public DbCommand GetInsertCommand() {
 
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        
        public void Update(){
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider){
        
            
            if(this._dirtyColumns.Count>0){
                _repo.Update(this,provider);
                _dirtyColumns.Clear();    
            }
            OnSaved();
       }
 
        public void Add(){
            Add(_db.DataProvider);
        }
        
        
       
        public void Add(IDataProvider provider){

            
            var key=KeyValue();
            if(key==null){
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }else{
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
                
        
        public void Save() {
            Save(_db.DataProvider);
        }      
        public void Save(IDataProvider provider) {
            
           
            if (_isNew) {
                Add(provider);
                
            } else {
                Update(provider);
            }
            
        }

        

        public void Delete(IDataProvider provider) {
                   
                 
            _repo.Delete(KeyValue());
            
                    }


        public void Delete() {
            Delete(_db.DataProvider);
        }


        public static void Delete(Expression<Func<ASync_RealityFare, bool>> expression) {
            var repo = GetRepo();
            
       
            
            repo.DeleteMany(expression);
            
        }

        

        public void Load(IDataReader rdr) {
            Load(rdr, true);
        }
        public void Load(IDataReader rdr, bool closeReader) {
            if (rdr.Read()) {

                try {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                } catch {
                    SetIsLoaded(false);
                    throw;
                }
            }else{
                SetIsLoaded(false);
            }

            if (closeReader)
                rdr.Dispose();
        }
        

    } 
    
    
    /// <summary>
    /// A class which represents the ASync_CityCode table in the ASync Database.
    /// </summary>
    public partial class ASync_CityCode: IActiveRecord
    {
    
        #region Built-in testing
        static TestRepository<ASync_CityCode> _testRepo;
        

        
        static void SetTestRepo(){
            _testRepo = _testRepo ?? new TestRepository<ASync_CityCode>(new ASync.eTermAddIn.ASyncDB());
        }
        public static void ResetTestRepo(){
            _testRepo = null;
            SetTestRepo();
        }
        public static void Setup(List<ASync_CityCode> testlist){
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }
        public static void Setup(ASync_CityCode item) {
            SetTestRepo();
            _testRepo._items.Add(item);
        }
        public static void Setup(int testItems) {
            SetTestRepo();
            for(int i=0;i<testItems;i++){
                ASync_CityCode item=new ASync_CityCode();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;


        #endregion

        IRepository<ASync_CityCode> _repo;
        ITable tbl;
        bool _isNew;
        public bool IsNew(){
            return _isNew;
        }
        
        public void SetIsLoaded(bool isLoaded){
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        public void SetIsNew(bool isNew){
            _isNew=isNew;
        }
        bool _isLoaded;
        public bool IsLoaded(){
            return _isLoaded;
        }
                
        List<IColumn> _dirtyColumns;
        public bool IsDirty(){
            return _dirtyColumns.Count>0;
        }
        
        public List<IColumn> GetDirtyColumns (){
            return _dirtyColumns;
        }

        ASync.eTermAddIn.ASyncDB _db;
        public ASync_CityCode(string connectionString, string providerName) {

            _db=new ASync.eTermAddIn.ASyncDB(connectionString, providerName);
            Init();            
         }
         
        public ASync_CityCode(string connectionString)
            : this(connectionString, @"System.Data.SqlClient") { 
            
        }
         
        void Init(){
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode){
                ASync_CityCode.SetTestRepo();
                _repo=_testRepo;
            }else{
                _repo = new SubSonicRepository<ASync_CityCode>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
            OnCreated();       

        }
        
        public ASync_CityCode(){
             _db=new ASync.eTermAddIn.ASyncDB();
            Init();            
        }
        
       
        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaved();
        
        partial void OnChanged();
        
        public IList<IColumn> Columns{
            get{
                return tbl.Columns;
            }
        }

        public ASync_CityCode(Expression<Func<ASync_CityCode, bool>> expression):this() {

            SetIsLoaded(_repo.Load(this,expression));
        }
        
       
        
        internal static IRepository<ASync_CityCode> GetRepo(string connectionString, string providerName){
            ASync.eTermAddIn.ASyncDB db;
            if(String.IsNullOrEmpty(connectionString)){
                db=new ASync.eTermAddIn.ASyncDB();
            }else{
                db=new ASync.eTermAddIn.ASyncDB(connectionString, providerName);
            }
            IRepository<ASync_CityCode> _repo;
            
            if(db.TestMode){
                ASync_CityCode.SetTestRepo();
                _repo=_testRepo;
            }else{
                _repo = new SubSonicRepository<ASync_CityCode>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<ASync_CityCode> GetRepo(){
            return GetRepo("","");
        }
        
        public static ASync_CityCode SingleOrDefault(Expression<Func<ASync_CityCode, bool>> expression) {

            var repo = GetRepo();
            var results=repo.Find(expression);
            ASync_CityCode single=null;
            if(results.Count() > 0){
                single=results.ToList()[0];
                single.OnLoaded();
                single.SetIsLoaded(true);
                single.SetIsNew(false);
            }

            return single;
        }      
        
        public static ASync_CityCode SingleOrDefault(Expression<Func<ASync_CityCode, bool>> expression,string connectionString, string providerName) {
            var repo = GetRepo(connectionString,providerName);
            var results=repo.Find(expression);
            ASync_CityCode single=null;
            if(results.Count() > 0){
                single=results.ToList()[0];
            }

            return single;


        }
        
        
        public static bool Exists(Expression<Func<ASync_CityCode, bool>> expression,string connectionString, string providerName) {
           
            return All(connectionString,providerName).Any(expression);
        }        
        public static bool Exists(Expression<Func<ASync_CityCode, bool>> expression) {
           
            return All().Any(expression);
        }        

        public static IList<ASync_CityCode> Find(Expression<Func<ASync_CityCode, bool>> expression) {
            
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<ASync_CityCode> Find(Expression<Func<ASync_CityCode, bool>> expression,string connectionString, string providerName) {

            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();

        }
        public static IQueryable<ASync_CityCode> All(string connectionString, string providerName) {
            return GetRepo(connectionString,providerName).GetAll();
        }
        public static IQueryable<ASync_CityCode> All() {
            return GetRepo().GetAll();
        }
        
        public static PagedList<ASync_CityCode> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<ASync_CityCode> GetPaged(string sortBy, int pageIndex, int pageSize) {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<ASync_CityCode> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
            
        }


        public static PagedList<ASync_CityCode> GetPaged(int pageIndex, int pageSize) {
            return GetRepo().GetPaged(pageIndex, pageSize);
            
        }

        public string KeyName()
        {
            return "CityCode";
        }

        public object KeyValue()
        {
            return this.CityCode;
        }
        
        public void SetKeyValue(object value) {
            if (value != null && value!=DBNull.Value) {
                var settable = value.ChangeTypeTo<string>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
        public override string ToString(){
                            return this.CityId.ToString();
                    }

        public override bool Equals(object obj){
            if(obj.GetType()==typeof(ASync_CityCode)){
                ASync_CityCode compare=(ASync_CityCode)obj;
                return compare.KeyValue()==this.KeyValue();
            }else{
                return base.Equals(obj);
            }
        }

        public string DescriptorValue()
        {
                            return this.CityId.ToString();
                    }

        public string DescriptorColumn() {
            return "CityId";
        }
        public static string GetKeyColumn()
        {
            return "CityCode";
        }        
        public static string GetDescriptorColumn()
        {
            return "CityId";
        }
        
        #region ' Foreign Keys '
        #endregion
        

        string _CityId;
        public string CityId
        {
            get { return _CityId; }
            set
            {
                if(_CityId!=value){
                    _CityId=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CityId");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        string _CityCode;
        public string CityCode
        {
            get { return _CityCode; }
            set
            {
                if(_CityCode!=value){
                    _CityCode=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CityCode");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        string _CityName;
        public string CityName
        {
            get { return _CityName; }
            set
            {
                if(_CityName!=value){
                    _CityName=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CityName");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        string _CityPinYin;
        public string CityPinYin
        {
            get { return _CityPinYin; }
            set
            {
                if(_CityPinYin!=value){
                    _CityPinYin=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CityPinYin");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        string _CountryCode;
        public string CountryCode
        {
            get { return _CountryCode; }
            set
            {
                if(_CountryCode!=value){
                    _CountryCode=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="CountryCode");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        bool? _Flag;
        public bool? Flag
        {
            get { return _Flag; }
            set
            {
                if(_Flag!=value){
                    _Flag=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Flag");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        string _ParentId;
        public string ParentId
        {
            get { return _ParentId; }
            set
            {
                if(_ParentId!=value){
                    _ParentId=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="ParentId");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }



        public DbCommand GetUpdateCommand() {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }
        public DbCommand GetInsertCommand() {
 
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        
        public void Update(){
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider){
        
            
            if(this._dirtyColumns.Count>0){
                _repo.Update(this,provider);
                _dirtyColumns.Clear();    
            }
            OnSaved();
       }
 
        public void Add(){
            Add(_db.DataProvider);
        }
        
        
       
        public void Add(IDataProvider provider){

            
            var key=KeyValue();
            if(key==null){
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }else{
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
                
        
        public void Save() {
            Save(_db.DataProvider);
        }      
        public void Save(IDataProvider provider) {
            
           
            if (_isNew) {
                Add(provider);
                
            } else {
                Update(provider);
            }
            
        }

        

        public void Delete(IDataProvider provider) {
                   
                 
            _repo.Delete(KeyValue());
            
                    }


        public void Delete() {
            Delete(_db.DataProvider);
        }


        public static void Delete(Expression<Func<ASync_CityCode, bool>> expression) {
            var repo = GetRepo();
            
       
            
            repo.DeleteMany(expression);
            
        }

        

        public void Load(IDataReader rdr) {
            Load(rdr, true);
        }
        public void Load(IDataReader rdr, bool closeReader) {
            if (rdr.Read()) {

                try {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                } catch {
                    SetIsLoaded(false);
                    throw;
                }
            }else{
                SetIsLoaded(false);
            }

            if (closeReader)
                rdr.Dispose();
        }
        

    } 
    
    
    /// <summary>
    /// A class which represents the Async_Log table in the ASync Database.
    /// </summary>
    public partial class Async_Log: IActiveRecord
    {
    
        #region Built-in testing
        static TestRepository<Async_Log> _testRepo;
        

        
        static void SetTestRepo(){
            _testRepo = _testRepo ?? new TestRepository<Async_Log>(new ASync.eTermAddIn.ASyncDB());
        }
        public static void ResetTestRepo(){
            _testRepo = null;
            SetTestRepo();
        }
        public static void Setup(List<Async_Log> testlist){
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }
        public static void Setup(Async_Log item) {
            SetTestRepo();
            _testRepo._items.Add(item);
        }
        public static void Setup(int testItems) {
            SetTestRepo();
            for(int i=0;i<testItems;i++){
                Async_Log item=new Async_Log();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;


        #endregion

        IRepository<Async_Log> _repo;
        ITable tbl;
        bool _isNew;
        public bool IsNew(){
            return _isNew;
        }
        
        public void SetIsLoaded(bool isLoaded){
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        public void SetIsNew(bool isNew){
            _isNew=isNew;
        }
        bool _isLoaded;
        public bool IsLoaded(){
            return _isLoaded;
        }
                
        List<IColumn> _dirtyColumns;
        public bool IsDirty(){
            return _dirtyColumns.Count>0;
        }
        
        public List<IColumn> GetDirtyColumns (){
            return _dirtyColumns;
        }

        ASync.eTermAddIn.ASyncDB _db;
        public Async_Log(string connectionString, string providerName) {

            _db=new ASync.eTermAddIn.ASyncDB(connectionString, providerName);
            Init();            
         }
         
        public Async_Log(string connectionString)
            : this(connectionString, @"System.Data.SqlClient") { 
            
        }
         
        void Init(){
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode){
                Async_Log.SetTestRepo();
                _repo=_testRepo;
            }else{
                _repo = new SubSonicRepository<Async_Log>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
            OnCreated();       

        }
        
        public Async_Log(){
             _db=new ASync.eTermAddIn.ASyncDB();
            Init();            
        }
        
       
        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaved();
        
        partial void OnChanged();
        
        public IList<IColumn> Columns{
            get{
                return tbl.Columns;
            }
        }

        public Async_Log(Expression<Func<Async_Log, bool>> expression):this() {

            SetIsLoaded(_repo.Load(this,expression));
        }
        
       
        
        internal static IRepository<Async_Log> GetRepo(string connectionString, string providerName){
            ASync.eTermAddIn.ASyncDB db;
            if(String.IsNullOrEmpty(connectionString)){
                db=new ASync.eTermAddIn.ASyncDB();
            }else{
                db=new ASync.eTermAddIn.ASyncDB(connectionString, providerName);
            }
            IRepository<Async_Log> _repo;
            
            if(db.TestMode){
                Async_Log.SetTestRepo();
                _repo=_testRepo;
            }else{
                _repo = new SubSonicRepository<Async_Log>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<Async_Log> GetRepo(){
            return GetRepo("","");
        }
        
        public static Async_Log SingleOrDefault(Expression<Func<Async_Log, bool>> expression) {

            var repo = GetRepo();
            var results=repo.Find(expression);
            Async_Log single=null;
            if(results.Count() > 0){
                single=results.ToList()[0];
                single.OnLoaded();
                single.SetIsLoaded(true);
                single.SetIsNew(false);
            }

            return single;
        }      
        
        public static Async_Log SingleOrDefault(Expression<Func<Async_Log, bool>> expression,string connectionString, string providerName) {
            var repo = GetRepo(connectionString,providerName);
            var results=repo.Find(expression);
            Async_Log single=null;
            if(results.Count() > 0){
                single=results.ToList()[0];
            }

            return single;


        }
        
        
        public static bool Exists(Expression<Func<Async_Log, bool>> expression,string connectionString, string providerName) {
           
            return All(connectionString,providerName).Any(expression);
        }        
        public static bool Exists(Expression<Func<Async_Log, bool>> expression) {
           
            return All().Any(expression);
        }        

        public static IList<Async_Log> Find(Expression<Func<Async_Log, bool>> expression) {
            
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<Async_Log> Find(Expression<Func<Async_Log, bool>> expression,string connectionString, string providerName) {

            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();

        }
        public static IQueryable<Async_Log> All(string connectionString, string providerName) {
            return GetRepo(connectionString,providerName).GetAll();
        }
        public static IQueryable<Async_Log> All() {
            return GetRepo().GetAll();
        }
        
        public static PagedList<Async_Log> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<Async_Log> GetPaged(string sortBy, int pageIndex, int pageSize) {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<Async_Log> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
            
        }


        public static PagedList<Async_Log> GetPaged(int pageIndex, int pageSize) {
            return GetRepo().GetPaged(pageIndex, pageSize);
            
        }

        public string KeyName()
        {
            return "Id";
        }

        public object KeyValue()
        {
            return this.Id;
        }
        
        public void SetKeyValue(object value) {
            if (value != null && value!=DBNull.Value) {
                var settable = value.ChangeTypeTo<long>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
        public override string ToString(){
                            return this.eTermSession.ToString();
                    }

        public override bool Equals(object obj){
            if(obj.GetType()==typeof(Async_Log)){
                Async_Log compare=(Async_Log)obj;
                return compare.KeyValue()==this.KeyValue();
            }else{
                return base.Equals(obj);
            }
        }

        public string DescriptorValue()
        {
                            return this.eTermSession.ToString();
                    }

        public string DescriptorColumn() {
            return "eTermSession";
        }
        public static string GetKeyColumn()
        {
            return "Id";
        }        
        public static string GetDescriptorColumn()
        {
            return "eTermSession";
        }
        
        #region ' Foreign Keys '
        #endregion
        

        long _Id;
        public long Id
        {
            get { return _Id; }
            set
            {
                if(_Id!=value){
                    _Id=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Id");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        string _eTermSession;
        public string eTermSession
        {
            get { return _eTermSession; }
            set
            {
                if(_eTermSession!=value){
                    _eTermSession=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="eTermSession");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        string _ClientSession;
        public string ClientSession
        {
            get { return _ClientSession; }
            set
            {
                if(_ClientSession!=value){
                    _ClientSession=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="ClientSession");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        string _ASynCommand;
        public string ASynCommand
        {
            get { return _ASynCommand; }
            set
            {
                if(_ASynCommand!=value){
                    _ASynCommand=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="ASynCommand");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        string _ASyncResult;
        public string ASyncResult
        {
            get { return _ASyncResult; }
            set
            {
                if(_ASyncResult!=value){
                    _ASyncResult=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="ASyncResult");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        DateTime? _LogDate;
        public DateTime? LogDate
        {
            get { return _LogDate; }
            set
            {
                if(_LogDate!=value){
                    _LogDate=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="LogDate");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }



        public DbCommand GetUpdateCommand() {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }
        public DbCommand GetInsertCommand() {
 
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        
        public void Update(){
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider){
        
            
            if(this._dirtyColumns.Count>0){
                _repo.Update(this,provider);
                _dirtyColumns.Clear();    
            }
            OnSaved();
       }
 
        public void Add(){
            Add(_db.DataProvider);
        }
        
        
       
        public void Add(IDataProvider provider){

            
            var key=KeyValue();
            if(key==null){
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }else{
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
                
        
        public void Save() {
            Save(_db.DataProvider);
        }      
        public void Save(IDataProvider provider) {
            
           
            if (_isNew) {
                Add(provider);
                
            } else {
                Update(provider);
            }
            
        }

        

        public void Delete(IDataProvider provider) {
                   
                 
            _repo.Delete(KeyValue());
            
                    }


        public void Delete() {
            Delete(_db.DataProvider);
        }


        public static void Delete(Expression<Func<Async_Log, bool>> expression) {
            var repo = GetRepo();
            
       
            
            repo.DeleteMany(expression);
            
        }

        

        public void Load(IDataReader rdr) {
            Load(rdr, true);
        }
        public void Load(IDataReader rdr, bool closeReader) {
            if (rdr.Read()) {

                try {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                } catch {
                    SetIsLoaded(false);
                    throw;
                }
            }else{
                SetIsLoaded(false);
            }

            if (closeReader)
                rdr.Dispose();
        }
        

    } 
    
    
    /// <summary>
    /// A class which represents the Async_PNR table in the ASync Database.
    /// </summary>
    public partial class Async_PNR: IActiveRecord
    {
    
        #region Built-in testing
        static TestRepository<Async_PNR> _testRepo;
        

        
        static void SetTestRepo(){
            _testRepo = _testRepo ?? new TestRepository<Async_PNR>(new ASync.eTermAddIn.ASyncDB());
        }
        public static void ResetTestRepo(){
            _testRepo = null;
            SetTestRepo();
        }
        public static void Setup(List<Async_PNR> testlist){
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }
        public static void Setup(Async_PNR item) {
            SetTestRepo();
            _testRepo._items.Add(item);
        }
        public static void Setup(int testItems) {
            SetTestRepo();
            for(int i=0;i<testItems;i++){
                Async_PNR item=new Async_PNR();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;


        #endregion

        IRepository<Async_PNR> _repo;
        ITable tbl;
        bool _isNew;
        public bool IsNew(){
            return _isNew;
        }
        
        public void SetIsLoaded(bool isLoaded){
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        public void SetIsNew(bool isNew){
            _isNew=isNew;
        }
        bool _isLoaded;
        public bool IsLoaded(){
            return _isLoaded;
        }
                
        List<IColumn> _dirtyColumns;
        public bool IsDirty(){
            return _dirtyColumns.Count>0;
        }
        
        public List<IColumn> GetDirtyColumns (){
            return _dirtyColumns;
        }

        ASync.eTermAddIn.ASyncDB _db;
        public Async_PNR(string connectionString, string providerName) {

            _db=new ASync.eTermAddIn.ASyncDB(connectionString, providerName);
            Init();            
         }
         
        public Async_PNR(string connectionString)
            : this(connectionString, @"System.Data.SqlClient") { 
            
        }
         
        void Init(){
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode){
                Async_PNR.SetTestRepo();
                _repo=_testRepo;
            }else{
                _repo = new SubSonicRepository<Async_PNR>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
            OnCreated();       

        }
        
        public Async_PNR(){
             _db=new ASync.eTermAddIn.ASyncDB();
            Init();            
        }
        
       
        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaved();
        
        partial void OnChanged();
        
        public IList<IColumn> Columns{
            get{
                return tbl.Columns;
            }
        }

        public Async_PNR(Expression<Func<Async_PNR, bool>> expression):this() {

            SetIsLoaded(_repo.Load(this,expression));
        }
        
       
        
        internal static IRepository<Async_PNR> GetRepo(string connectionString, string providerName){
            ASync.eTermAddIn.ASyncDB db;
            if(String.IsNullOrEmpty(connectionString)){
                db=new ASync.eTermAddIn.ASyncDB();
            }else{
                db=new ASync.eTermAddIn.ASyncDB(connectionString, providerName);
            }
            IRepository<Async_PNR> _repo;
            
            if(db.TestMode){
                Async_PNR.SetTestRepo();
                _repo=_testRepo;
            }else{
                _repo = new SubSonicRepository<Async_PNR>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<Async_PNR> GetRepo(){
            return GetRepo("","");
        }
        
        public static Async_PNR SingleOrDefault(Expression<Func<Async_PNR, bool>> expression) {

            var repo = GetRepo();
            var results=repo.Find(expression);
            Async_PNR single=null;
            if(results.Count() > 0){
                single=results.ToList()[0];
                single.OnLoaded();
                single.SetIsLoaded(true);
                single.SetIsNew(false);
            }

            return single;
        }      
        
        public static Async_PNR SingleOrDefault(Expression<Func<Async_PNR, bool>> expression,string connectionString, string providerName) {
            var repo = GetRepo(connectionString,providerName);
            var results=repo.Find(expression);
            Async_PNR single=null;
            if(results.Count() > 0){
                single=results.ToList()[0];
            }

            return single;


        }
        
        
        public static bool Exists(Expression<Func<Async_PNR, bool>> expression,string connectionString, string providerName) {
           
            return All(connectionString,providerName).Any(expression);
        }        
        public static bool Exists(Expression<Func<Async_PNR, bool>> expression) {
           
            return All().Any(expression);
        }        

        public static IList<Async_PNR> Find(Expression<Func<Async_PNR, bool>> expression) {
            
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<Async_PNR> Find(Expression<Func<Async_PNR, bool>> expression,string connectionString, string providerName) {

            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();

        }
        public static IQueryable<Async_PNR> All(string connectionString, string providerName) {
            return GetRepo(connectionString,providerName).GetAll();
        }
        public static IQueryable<Async_PNR> All() {
            return GetRepo().GetAll();
        }
        
        public static PagedList<Async_PNR> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<Async_PNR> GetPaged(string sortBy, int pageIndex, int pageSize) {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<Async_PNR> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
            
        }


        public static PagedList<Async_PNR> GetPaged(int pageIndex, int pageSize) {
            return GetRepo().GetPaged(pageIndex, pageSize);
            
        }

        public string KeyName()
        {
            return "IdentityId";
        }

        public object KeyValue()
        {
            return this.IdentityId;
        }
        
        public void SetKeyValue(object value) {
            if (value != null && value!=DBNull.Value) {
                var settable = value.ChangeTypeTo<long>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
        public override string ToString(){
                            return this.ClientSession.ToString();
                    }

        public override bool Equals(object obj){
            if(obj.GetType()==typeof(Async_PNR)){
                Async_PNR compare=(Async_PNR)obj;
                return compare.KeyValue()==this.KeyValue();
            }else{
                return base.Equals(obj);
            }
        }

        public string DescriptorValue()
        {
                            return this.ClientSession.ToString();
                    }

        public string DescriptorColumn() {
            return "ClientSession";
        }
        public static string GetKeyColumn()
        {
            return "IdentityId";
        }        
        public static string GetDescriptorColumn()
        {
            return "ClientSession";
        }
        
        #region ' Foreign Keys '
        #endregion
        

        long _IdentityId;
        public long IdentityId
        {
            get { return _IdentityId; }
            set
            {
                if(_IdentityId!=value){
                    _IdentityId=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="IdentityId");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        string _ClientSession;
        public string ClientSession
        {
            get { return _ClientSession; }
            set
            {
                if(_ClientSession!=value){
                    _ClientSession=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="ClientSession");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        string _PnrCode;
        public string PnrCode
        {
            get { return _PnrCode; }
            set
            {
                if(_PnrCode!=value){
                    _PnrCode=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="PnrCode");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        string _SourcePnr;
        public string SourcePnr
        {
            get { return _SourcePnr; }
            set
            {
                if(_SourcePnr!=value){
                    _SourcePnr=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="SourcePnr");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        DateTime? _UpdateDate;
        public DateTime? UpdateDate
        {
            get { return _UpdateDate; }
            set
            {
                if(_UpdateDate!=value){
                    _UpdateDate=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="UpdateDate");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }



        public DbCommand GetUpdateCommand() {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }
        public DbCommand GetInsertCommand() {
 
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        
        public void Update(){
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider){
        
            
            if(this._dirtyColumns.Count>0){
                _repo.Update(this,provider);
                _dirtyColumns.Clear();    
            }
            OnSaved();
       }
 
        public void Add(){
            Add(_db.DataProvider);
        }
        
        
       
        public void Add(IDataProvider provider){

            
            var key=KeyValue();
            if(key==null){
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }else{
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
                
        
        public void Save() {
            Save(_db.DataProvider);
        }      
        public void Save(IDataProvider provider) {
            
           
            if (_isNew) {
                Add(provider);
                
            } else {
                Update(provider);
            }
            
        }

        

        public void Delete(IDataProvider provider) {
                   
                 
            _repo.Delete(KeyValue());
            
                    }


        public void Delete() {
            Delete(_db.DataProvider);
        }


        public static void Delete(Expression<Func<Async_PNR, bool>> expression) {
            var repo = GetRepo();
            
       
            
            repo.DeleteMany(expression);
            
        }

        

        public void Load(IDataReader rdr) {
            Load(rdr, true);
        }
        public void Load(IDataReader rdr, bool closeReader) {
            if (rdr.Read()) {

                try {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                } catch {
                    SetIsLoaded(false);
                    throw;
                }
            }else{
                SetIsLoaded(false);
            }

            if (closeReader)
                rdr.Dispose();
        }
        

    } 
    
    
    /// <summary>
    /// A class which represents the ASync_AirLine table in the ASync Database.
    /// </summary>
    public partial class ASync_AirLine: IActiveRecord
    {
    
        #region Built-in testing
        static TestRepository<ASync_AirLine> _testRepo;
        

        
        static void SetTestRepo(){
            _testRepo = _testRepo ?? new TestRepository<ASync_AirLine>(new ASync.eTermAddIn.ASyncDB());
        }
        public static void ResetTestRepo(){
            _testRepo = null;
            SetTestRepo();
        }
        public static void Setup(List<ASync_AirLine> testlist){
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }
        public static void Setup(ASync_AirLine item) {
            SetTestRepo();
            _testRepo._items.Add(item);
        }
        public static void Setup(int testItems) {
            SetTestRepo();
            for(int i=0;i<testItems;i++){
                ASync_AirLine item=new ASync_AirLine();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;


        #endregion

        IRepository<ASync_AirLine> _repo;
        ITable tbl;
        bool _isNew;
        public bool IsNew(){
            return _isNew;
        }
        
        public void SetIsLoaded(bool isLoaded){
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        public void SetIsNew(bool isNew){
            _isNew=isNew;
        }
        bool _isLoaded;
        public bool IsLoaded(){
            return _isLoaded;
        }
                
        List<IColumn> _dirtyColumns;
        public bool IsDirty(){
            return _dirtyColumns.Count>0;
        }
        
        public List<IColumn> GetDirtyColumns (){
            return _dirtyColumns;
        }

        ASync.eTermAddIn.ASyncDB _db;
        public ASync_AirLine(string connectionString, string providerName) {

            _db=new ASync.eTermAddIn.ASyncDB(connectionString, providerName);
            Init();            
         }
         
        public ASync_AirLine(string connectionString)
            : this(connectionString, @"System.Data.SqlClient") { 
            
        }
         
        void Init(){
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode){
                ASync_AirLine.SetTestRepo();
                _repo=_testRepo;
            }else{
                _repo = new SubSonicRepository<ASync_AirLine>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
            OnCreated();       

        }
        
        public ASync_AirLine(){
             _db=new ASync.eTermAddIn.ASyncDB();
            Init();            
        }
        
       
        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaved();
        
        partial void OnChanged();
        
        public IList<IColumn> Columns{
            get{
                return tbl.Columns;
            }
        }

        public ASync_AirLine(Expression<Func<ASync_AirLine, bool>> expression):this() {

            SetIsLoaded(_repo.Load(this,expression));
        }
        
       
        
        internal static IRepository<ASync_AirLine> GetRepo(string connectionString, string providerName){
            ASync.eTermAddIn.ASyncDB db;
            if(String.IsNullOrEmpty(connectionString)){
                db=new ASync.eTermAddIn.ASyncDB();
            }else{
                db=new ASync.eTermAddIn.ASyncDB(connectionString, providerName);
            }
            IRepository<ASync_AirLine> _repo;
            
            if(db.TestMode){
                ASync_AirLine.SetTestRepo();
                _repo=_testRepo;
            }else{
                _repo = new SubSonicRepository<ASync_AirLine>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<ASync_AirLine> GetRepo(){
            return GetRepo("","");
        }
        
        public static ASync_AirLine SingleOrDefault(Expression<Func<ASync_AirLine, bool>> expression) {

            var repo = GetRepo();
            var results=repo.Find(expression);
            ASync_AirLine single=null;
            if(results.Count() > 0){
                single=results.ToList()[0];
                single.OnLoaded();
                single.SetIsLoaded(true);
                single.SetIsNew(false);
            }

            return single;
        }      
        
        public static ASync_AirLine SingleOrDefault(Expression<Func<ASync_AirLine, bool>> expression,string connectionString, string providerName) {
            var repo = GetRepo(connectionString,providerName);
            var results=repo.Find(expression);
            ASync_AirLine single=null;
            if(results.Count() > 0){
                single=results.ToList()[0];
            }

            return single;


        }
        
        
        public static bool Exists(Expression<Func<ASync_AirLine, bool>> expression,string connectionString, string providerName) {
           
            return All(connectionString,providerName).Any(expression);
        }        
        public static bool Exists(Expression<Func<ASync_AirLine, bool>> expression) {
           
            return All().Any(expression);
        }        

        public static IList<ASync_AirLine> Find(Expression<Func<ASync_AirLine, bool>> expression) {
            
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<ASync_AirLine> Find(Expression<Func<ASync_AirLine, bool>> expression,string connectionString, string providerName) {

            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();

        }
        public static IQueryable<ASync_AirLine> All(string connectionString, string providerName) {
            return GetRepo(connectionString,providerName).GetAll();
        }
        public static IQueryable<ASync_AirLine> All() {
            return GetRepo().GetAll();
        }
        
        public static PagedList<ASync_AirLine> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<ASync_AirLine> GetPaged(string sortBy, int pageIndex, int pageSize) {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<ASync_AirLine> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
            
        }


        public static PagedList<ASync_AirLine> GetPaged(int pageIndex, int pageSize) {
            return GetRepo().GetPaged(pageIndex, pageSize);
            
        }

        public string KeyName()
        {
            return "AirLineCode";
        }

        public object KeyValue()
        {
            return this.AirLineCode;
        }
        
        public void SetKeyValue(object value) {
            if (value != null && value!=DBNull.Value) {
                var settable = value.ChangeTypeTo<string>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
        public override string ToString(){
                            return this.AirLineCode.ToString();
                    }

        public override bool Equals(object obj){
            if(obj.GetType()==typeof(ASync_AirLine)){
                ASync_AirLine compare=(ASync_AirLine)obj;
                return compare.KeyValue()==this.KeyValue();
            }else{
                return base.Equals(obj);
            }
        }

        public string DescriptorValue()
        {
                            return this.AirLineCode.ToString();
                    }

        public string DescriptorColumn() {
            return "AirLineCode";
        }
        public static string GetKeyColumn()
        {
            return "AirLineCode";
        }        
        public static string GetDescriptorColumn()
        {
            return "AirLineCode";
        }
        
        #region ' Foreign Keys '
        #endregion
        

        string _AirLineCode;
        public string AirLineCode
        {
            get { return _AirLineCode; }
            set
            {
                if(_AirLineCode!=value){
                    _AirLineCode=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="AirLineCode");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        string _ChineseName;
        public string ChineseName
        {
            get { return _ChineseName; }
            set
            {
                if(_ChineseName!=value){
                    _ChineseName=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="ChineseName");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        string _EnglishName;
        public string EnglishName
        {
            get { return _EnglishName; }
            set
            {
                if(_EnglishName!=value){
                    _EnglishName=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="EnglishName");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }



        public DbCommand GetUpdateCommand() {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }
        public DbCommand GetInsertCommand() {
 
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        
        public void Update(){
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider){
        
            
            if(this._dirtyColumns.Count>0){
                _repo.Update(this,provider);
                _dirtyColumns.Clear();    
            }
            OnSaved();
       }
 
        public void Add(){
            Add(_db.DataProvider);
        }
        
        
       
        public void Add(IDataProvider provider){

            
            var key=KeyValue();
            if(key==null){
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }else{
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
                
        
        public void Save() {
            Save(_db.DataProvider);
        }      
        public void Save(IDataProvider provider) {
            
           
            if (_isNew) {
                Add(provider);
                
            } else {
                Update(provider);
            }
            
        }

        

        public void Delete(IDataProvider provider) {
                   
                 
            _repo.Delete(KeyValue());
            
                    }


        public void Delete() {
            Delete(_db.DataProvider);
        }


        public static void Delete(Expression<Func<ASync_AirLine, bool>> expression) {
            var repo = GetRepo();
            
       
            
            repo.DeleteMany(expression);
            
        }

        

        public void Load(IDataReader rdr) {
            Load(rdr, true);
        }
        public void Load(IDataReader rdr, bool closeReader) {
            if (rdr.Read()) {

                try {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                } catch {
                    SetIsLoaded(false);
                    throw;
                }
            }else{
                SetIsLoaded(false);
            }

            if (closeReader)
                rdr.Dispose();
        }
        

    } 
}
