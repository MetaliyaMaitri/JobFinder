using JobFinder.Areas.Country.Models;
using JobFinder.Areas.State.Models;
using JobFinder.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace JobFinder.Areas.State.Controllers
{
    [Area("State")]
    [Route("State/{controller}/{action}")]
    public class StateController : Controller
    {
        #region Configuration
        private IConfiguration Configuration;
        public StateController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #endregion
        #region SelectAll

        public IActionResult StateList()
        {
            State_Base_DAL dal = new State_Base_DAL();
            return View(dal.StateSelectAll());

        }
        #endregion

        #region SelectAllSearch

        public IActionResult SearchState(StateSearchModel StateSearch)
        {
            try
            {
                String connectionStr = this.Configuration.GetConnectionString("myConnectionString");
                Database db = new SqlDatabase(connectionStr);
                DbCommand dbCmd = db.GetStoredProcCommand("PR_State_Search");
                db.AddInParameter(dbCmd, "@CountryName", (DbType)SqlDbType.VarChar, StateSearch.CountryName);
                db.AddInParameter(dbCmd, "@StateName", (DbType)SqlDbType.VarChar, StateSearch.StateName);
                db.AddInParameter(dbCmd, "@StateCode", (DbType)SqlDbType.VarChar, StateSearch.StateCode);



                using (IDataReader dr = db.ExecuteReader(dbCmd))
                {
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    List<StateModel> modelList = new List<StateModel>();

                    foreach (DataRow row in dt.Rows)
                    {
                        StateModel state = new StateModel();
                        state.StateID = Convert.ToInt32(row["StateID"]);
                        state.CountryName = row["CountryName"].ToString();
                        state.StateName = row["StateName"].ToString();
                        state.StateCode = row["StateCode"].ToString();
                        state.Created = Convert.ToDateTime(row["Created"]);
                        state.Modified = Convert.ToDateTime(row["Modified"]);
                        // Set other properties here

                        modelList.Add(state);
                    }

                    return View("StateList", modelList);
                }
            }
            catch (Exception ex)
            {

                return null;

            }
        }
        #endregion

        #region #SetStateDropDownList
        public void SetCountryDropDownList()
        {
            State_Main_DAL lOC_DAL = new State_Main_DAL();
            DataTable dt = lOC_DAL.Set_Country_DropDownList();
            List<LOC_CountryDropDownModel> countryDropDownModelList = new List<LOC_CountryDropDownModel>();
            foreach (DataRow data in dt.Rows)
            {
                LOC_CountryDropDownModel countryDropDownModel = new LOC_CountryDropDownModel();
                countryDropDownModel.CountryID = Convert.ToInt32(data["CountryID"]);
                countryDropDownModel.CountryName = data["CountryName"].ToString();
                countryDropDownModelList.Add(countryDropDownModel);
            }
            ViewBag.SetCountryDropDownList = countryDropDownModelList;
        }
        #endregion
       


        #region LOC_StateAddEdit
        
        
        public IActionResult LOC_StateAddEdit(int StateID)
        {
            SetCountryDropDownList(); 
            if (StateID == 0 || StateID == null)
            {
                return View();
            }
            else
            {
                State_Base_DAL dal = new State_Base_DAL();
                StateModel model = dal.PR_LOC_State_SelectByPK((int)StateID);
                return View(model);
            }

        }
        #endregion

        #region SaveForAddEdit
        public IActionResult SaveForAddEdit(StateModel model)
        {
            SetCountryDropDownList();
            //if (ModelState.IsValid)
            //{
                bool ans = false;

                State_Base_DAL dal = new State_Base_DAL();
                if (model.StateID != 0)
                {
                    ans = dal.PR_LOC_State_Update(model);
                    TempData["message"] = "Record Updated Successfully";
                }
                else
                {
                    ans = dal.PR_LOC_State_Insert(model);
                    TempData["message"] = "Record Inserted Successfully";
                }
                if (ans)
                {
                    return RedirectToAction("StateList");
                }
                else
                {
                    return RedirectToAction("StateList");
                }
            //}
            //else
            //{

            //    return View("LOC_StateAddEdit");
            //}


        }
        #endregion

        #region StateDetails
        public IActionResult StateDetails(int StateID)
        {
            State_Base_DAL dal = new State_Base_DAL();
            StateModel model = dal.PR_LOC_State_SelectByPK(StateID);
            return View(model);

        }
        #endregion

        #region #StateDelete
        public IActionResult LOC_StateDelete(int StateID)
        {

            State_Base_DAL lOC_DAL = new State_Base_DAL();
            TempData["message"] = lOC_DAL.LOC_State_Delete(StateID); ;
            return RedirectToAction("StateList");
        }
        #endregion

        #region #Cancel
        public IActionResult Cancel()
        {
            return RedirectToAction("StateList");
        }
        #endregion
    }
}
