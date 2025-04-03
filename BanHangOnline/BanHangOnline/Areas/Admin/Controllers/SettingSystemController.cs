using BanHangOnline.Models;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{
    [Area("admin")]
    public class SettingSystemController : Controller
    {
        private readonly WebStoreDbContext _db;
        public SettingSystemController(WebStoreDbContext dbContext)
        {
            _db = dbContext;
        }
        public IActionResult Index()
        {
            SettingSystemViewModel item = new SettingSystemViewModel()
            {
                SettingTitle = GetValue("SettingTitle"),
                SettingEmail = GetValue("SettingEmail"),
                SettingHotline = GetValue("SettingHotline"),
                SettingTitleSeo = GetValue("SettingTitleSeo"),
                SettingDesSeo = GetValue("SettingDesSeo"),
                SettingKeySeo = GetValue("SettingKeySeo"),
                SettingLogo = GetValue("SettingLogo"),
            };
            return View(item);
        }

        [HttpPost]
        public IActionResult AddSetting(SettingSystemViewModel req)
        {
            var checkTitle = _db.SystemSetting.FirstOrDefault(x => x.SettingKey.Contains("SettingTitle"));
            SystemSetting? set = null;
            if (checkTitle == null
                && string.IsNullOrEmpty(checkTitle?.SettingValue))
            {
                set = new SystemSetting();
                set.SettingKey = "SettingTitle";
                set.SettingValue = req.SettingTitle;
                _db.SystemSetting.Add(set);
            }
            else
            {
                checkTitle.SettingValue = req.SettingTitle;
            }

            //logo
            var checkLogo = _db.SystemSetting.FirstOrDefault(x => x.SettingKey.Contains("SettingLogo"));
            if (checkLogo == null)
            {
                set = new SystemSetting();
                set.SettingKey = "SettingLogo";
                set.SettingValue = req.SettingLogo;
                _db.SystemSetting.Add(set);
            }
            else
            {
                checkLogo.SettingValue = req.SettingLogo;
            }

            //Email
            var email = _db.SystemSetting.FirstOrDefault(x => x.SettingKey.Contains("SettingEmail"));
            if (email == null)
            {
                set = new SystemSetting();
                set.SettingKey = "SettingEmail";
                set.SettingValue = req.SettingEmail;
                _db.SystemSetting.Add(set);
            }
            else
            {
                email.SettingValue = req.SettingEmail;
            }
            //Hotline
            var Hotline = _db.SystemSetting.FirstOrDefault(x => x.SettingKey.Contains("SettingHotline"));
            if (Hotline == null)
            {
                set = new SystemSetting();
                set.SettingKey = "SettingHotline";
                set.SettingValue = req.SettingHotline;
                _db.SystemSetting.Add(set);
            }
            else
            {
                Hotline.SettingValue = req.SettingHotline;
            }
            //TitleSeo
            var TitleSeo = _db.SystemSetting.FirstOrDefault(x => x.SettingKey.Contains("SettingTitleSeo"));
            if (TitleSeo == null)
            {
                set = new SystemSetting();
                set.SettingKey = "SettingTitleSeo";
                set.SettingValue = req.SettingTitleSeo;
                _db.SystemSetting.Add(set);
            }
            else
            {
                TitleSeo.SettingValue = req.SettingTitleSeo;
            }
            //DessSeo
            var DessSeo = _db.SystemSetting.FirstOrDefault(x => x.SettingKey.Contains("SettingDesSeo"));
            if (DessSeo == null)
            {
                set = new SystemSetting();
                set.SettingKey = "SettingDesSeo";
                set.SettingValue = req.SettingDesSeo;
                _db.SystemSetting.Add(set);
            }
            else
            {
                DessSeo.SettingValue = req.SettingDesSeo;
            }
            //KeySeo
            var KeySeo = _db.SystemSetting.FirstOrDefault(x => x.SettingKey.Contains("SettingKeySeo"));
            if (KeySeo == null)
            {
                set = new SystemSetting();
                set.SettingKey = "SettingKeySeo";
                set.SettingValue = req.SettingKeySeo;
                _db.SystemSetting.Add(set);
            }
            else
            {
                KeySeo.SettingValue = req.SettingKeySeo;
            }
            _db.SaveChanges();


            SettingSystemViewModel item = new SettingSystemViewModel()
            {
                SettingTitle = GetValue("SettingTitle"),
                SettingEmail = GetValue("SettingEmail"),
                SettingHotline = GetValue("SettingHotline"),
                SettingTitleSeo = GetValue("SettingTitleSeo"),
                SettingDesSeo = GetValue("SettingDesSeo"),
                SettingKeySeo = GetValue("SettingKeySeo"),
                SettingLogo = GetValue("SettingLogo"),
            };
            return View("Index", item);
        }


        private string GetValue(string key)
        {
            var item = _db.SystemSetting.SingleOrDefault(x => x.SettingKey == key);
            if (item != null)
            {
                return item.SettingValue;
            }
            return "";
        }
    }
}
