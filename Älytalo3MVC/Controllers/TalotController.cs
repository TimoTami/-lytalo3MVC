using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Älytalo3MVC.Models;
using Älytalo3MVC.ViewModel;

namespace Älytalo3MVC.Controllers
{
    public class TalotController : Controller
    {
        private ÄlytaloToinenEntities db = new ÄlytaloToinenEntities();

        // GET: Talot
        public ActionResult Index()
        {
            return View(db.Talot.ToList());
        }

        // GET: Talot/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Talo talo = db.Talot.Find(id);
            if (talo == null)
            {
                return HttpNotFound();
            }
            return View(talo);
        }

        // GET: Talot/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Talot/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaloID,TalonNimi")] Talo talo)
        {
            if (ModelState.IsValid)
            {
                db.Talot.Add(talo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(talo);
        }

        // GET: Talot/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Talo talo = db.Talot.Find(id);
            if (talo == null)
            {
                return HttpNotFound();
            }
            return View(talo);
        }

        // POST: Talot/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaloID,TalonNimi")] Talo talo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(talo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(talo);
        }

        // GET: Talot/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Talo talo = db.Talot.Find(id);
            if (talo == null)
            {
                return HttpNotFound();
            }
            return View(talo);
        }

        // POST: Talot/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            

            Talo talo = db.Talot.Find(id);
            

            foreach (var asiat in talo.Valo.ToList())
            {
                db.Valot.Remove(asiat);
            }
            foreach (var asiat in talo.Sauna.ToList())
            {
                db.Saunat.Remove(asiat);
            }
            foreach (var asiat in talo.TaloLampo.ToList())
            {
                db.TaloLampot.Remove(asiat);
            }

            
            db.Talot.Remove(talo);
            
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Timo2(/*TalonTiedotViewModel malli,*/ int? id)
        {
            List<TalonTiedotViewModel> modelli = new List<TalonTiedotViewModel>();
            ÄlytaloToinenEntities entities = new ÄlytaloToinenEntities();

            Talo talo = db.Talot.Find(id);
            //var valo = entities.Valot.Where(c=>c.TaloID==id).FirstOrDefault();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (talo == null)
            {
                return HttpNotFound();
            }
            try
            {
                List<Talo> talot = entities.Talot.ToList();
                //foreach (Talo t in talot)
                //{
                TalonTiedotViewModel view = new TalonTiedotViewModel();
                view.TalonNimi = talo.TalonNimi;
                view.TaloID = talo.TaloID;
                ÄlytaloToinenEntities dbvaloent = new ÄlytaloToinenEntities();
                modelli.Add(view);


                var saunat = dbvaloent.Saunat.Where(c => c.TaloID == id).ToList();
                foreach (Sauna s in saunat)
                {
                    TalonTiedotViewModel view1 = new TalonTiedotViewModel();
                    view1.SaunanNimi = s.SaunanNimi;
                    view1.SaunanNykyLampotila = s.SaunanNykyLampotila ?? 0;
                    view1.SaunanTila = s.SaunanTila;
                    view1.SaunaID = s.SaunaID;

                    ViewBag.TamaSaunaID = s.SaunaID;
                    modelli.Add(view1);
                }

                var valot = dbvaloent.Valot.Where(c => c.TaloID == id).ToList();
                foreach (Valo v in valot)
                {
                    TalonTiedotViewModel view2 = new TalonTiedotViewModel();
                    view2.ValonNimi = v.ValonNimi;
                    view2.ValonMaara = v.ValonMaara ?? 0;
                    view2.ValonTila = v.ValonTila;
                    view2.ValoID = v.ValoID;
                    ViewBag.TamaValoID = v.ValoID;

                    modelli.Add(view2);


                }
                var lammot = dbvaloent.TaloLampot.Where(c => c.TaloID == id).ToList();
                foreach (TaloLampo ft in lammot)
                {
                    TalonTiedotViewModel view3 = new TalonTiedotViewModel();
                    view3.HuoneenNimi = ft.HuoneenNimi;
                    view3.TalonNykyLampotila = ft.TalonNykyLampotila;
                    view3.TalonTavoiteLampotila = ft.TalonTavoiteLampotila;
                    view3.TaloLampoID = ft.TaloLampoID;
                    ViewBag.TamaTaloLampoID = ft.TaloLampoID;

                    modelli.Add(view3);
                }
                
                

                //}
            }
            finally
            {
                entities.Dispose();
            }
            ViewBag.TalonNimi = talo.TalonNimi;
            ViewBag.TamaTaloID = talo.TaloID;
            return View(modelli);
        }

        public ActionResult UusiValo(int id)
        {
            ViewBag.uusValoID = id;

            return View();
        }

        // POST: Talot/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UusiValo(int id ,[Bind(Include = "ValoID, ValonNimi, Valontila, ValonMaara ,TaloID")] Valo valo)
        {
            
            if (ModelState.IsValid)
            {

                valo.TaloID = id;
                ViewBag.LuominenTaloID = valo.TaloID;
                db.Valot.Add(valo);
                db.SaveChanges();
                return RedirectToAction("Timo2", new { id = valo.TaloID });
            }

            return View(valo);
        }
        public ActionResult UusiSauna(int id)
        {
            ViewBag.uusSaunaID = id;

            return View();
        }

        // POST: Talot/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UusiSauna(int id, [Bind(Include = "SaunaID, SaunanNimi, SaunanTila, SaunanNykyLampotila ,TaloID")] Sauna sauna)
        {
            if (ModelState.IsValid)
            {

                sauna.TaloID = id;
                db.Saunat.Add(sauna);
                db.SaveChanges();
                return RedirectToAction("Timo2", new { id = sauna.TaloID });
            }

            return View(sauna);
        }
        public ActionResult UusiLampo(int id)
        {
            ViewBag.uusLampoID = id;

            return View();
        }

        // POST: Talot/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UusiLampo(int id, [Bind(Include = "TaloLampoID, HuoneenNimi, TalonTavoiteLampotila, TalonNykyLampotila ,TaloID")] TaloLampo taloLampo)
        {
            if (ModelState.IsValid)
            {

                taloLampo.TaloID = id;
                db.TaloLampot.Add(taloLampo);
                db.SaveChanges();
                return RedirectToAction("Timo2", new { id = taloLampo.TaloID });
            }

            return View(taloLampo);
        }

        public ActionResult DeleteValo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Valo valo = db.Valot.Find(id);
            if (valo == null)
            {
                return HttpNotFound();
            }
            return View(valo);
        }

        // POST: Talot/Delete/5
        [HttpPost, ActionName("DeleteValo")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedValo(int id)
        {
            Valo valo = db.Valot.Find(id);
            var tamaValoTaloID = valo.TaloID;
            db.Valot.Remove(valo);
            db.SaveChanges();
            return RedirectToAction("Timo2", new { id = tamaValoTaloID });
        }

        public ActionResult DeleteSauna(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sauna sauna = db.Saunat.Find(id);
            if (sauna == null)
            {
                return HttpNotFound();
            }
            return View(sauna);
        }

        // POST: Talot/Delete/5
        [HttpPost, ActionName("DeleteSauna")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedSauna(int id)
        {
            Sauna sauna = db.Saunat.Find(id);
            var tamaSaunaTaloID = sauna.TaloID;
            db.Saunat.Remove(sauna);
            db.SaveChanges();
            return RedirectToAction("Timo2", new { id = tamaSaunaTaloID });
        }

        public ActionResult DeleteLampo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaloLampo taloLampo = db.TaloLampot.Find(id);
            if (taloLampo == null)
            {
                return HttpNotFound();
            }
            return View(taloLampo);
        }

        // POST: Talot/Delete/5
        [HttpPost, ActionName("DeleteLampo")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedLampo(int id)
        {
            TaloLampo taloLampo = db.TaloLampot.Find(id);
            var tamaTaloLampoTaloID = taloLampo.TaloID;
            db.TaloLampot.Remove(taloLampo);
            db.SaveChanges();
            return RedirectToAction("Timo2", new { id = tamaTaloLampoTaloID });
        }

        //protected override void DisposeValo(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}



        public ActionResult EditValo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Valo valo = db.Valot.Find(id);
            if (valo == null)
            {
                return HttpNotFound();
            }
            return View(valo);
        }

        // POST: Talot/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditValo(int id, [Bind(Include = "ValoID,ValonNimi,ValonTila,ValonMaara,TaloID,TalonNimi")] Valo valo)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(valo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Timo2", new { id = valo.TaloID });
            }
            return View(valo);
        }




        public ActionResult EditSauna(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sauna sauna = db.Saunat.Find(id);
            if (sauna == null)
            {
                return HttpNotFound();
            }
            return View(sauna);
        }

        // POST: Talot/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSauna(int id, [Bind(Include = "SaunaID,SaunanNimi,SaunanTila,SaunanNykyLampotila,TaloID,TalonNimi")] Sauna sauna)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(sauna).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Timo2", new { id = sauna.TaloID });
            }
            return View(sauna);
        }




        public ActionResult EditLampo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaloLampo taloLampo = db.TaloLampot.Find(id);
            ViewBag.taaID = taloLampo.TaloID;
            if (taloLampo == null)
            {
                return HttpNotFound();
            }
            return View(taloLampo);
        }

        // POST: Talot/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditLampo(int id, [Bind(Include = "TaloLampoID,HuoneenNimi,TalonNykyLampotila,TalonTavoiteLampotila,TaloID,TalonNimi")] TaloLampo taloLampo)
        {
            
            if (ModelState.IsValid)
            {
                
                db.Entry(taloLampo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Timo2", new { id = taloLampo.TaloID });
            }
            return View(taloLampo);
        }























        //public ActionResult TalonTiedotIndex22(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Talo talo = db.Talot.Find(id);
        //    if (talo == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    Talo view = new Talo();
        //    view.TaloID = talo.TaloID;
        //    view.TalonNimi = talo.TalonNimi;
        //    //view.SaunaID = talo.Sauna.SaunaID;
        //    //view.SaunanNimi = talo.Sauna.SaunanNimi;
        //    //view.SaunanTila = talo.Sauna?.SaunanTila;
        //    //view.SaunanNykyLampotila = talo.Sauna.SaunanNykyLampotila;
        //    //view.TaloLampoID = talo.TaloLampo.TaloLampoID;
        //    //view.HuoneenNimi = talo.TaloLampo.HuoneenNimi;
        //    //view.TalonTavoiteLampotila = talo.TaloLampo.TalonTavoiteLampotila;
        //    //view.TalonNykyLampotila = talo.TaloLampo.TalonNykyLampotila;
        //    //view.Valot.ValoID = talo.Valot?.ValoID;
        //    //view.Valot.ValonNimi = talo.Valot?.ValonNimi;
        //    //view.Valot.ValonTila = talo.Valot?.ValonTila;
        //    //view.Valot.ValonMaara = talo.Valot?.ValonMaara;


        //    //ViewBag.TalonNimi = new SelectList((from ta in db.Talo select new
        //    //{ TaloID = ta.TaloID, TalonNimi = ta.TalonNimi })
        //    //, "TaloID", "TalonNimi", view.TaloID);
        //    return View(view);
        //}

        // POST: Talot/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult TalonTiedotIndex(Talo model)
        //{
        //    Talo view = db.Talo.Find(model.TaloID);

        //    view.TalonNimi = model.TalonNimi;

        //    string valoID = (model.ValonNimi);
        //    if (valoID != null)
        //    {
        //        Valo vl = db.Valo.Find(valoID);
        //        view.ValoID = vl.ValoID;
        //    }
        //    //view.SaunaID = model.SaunaID;
        //    //view.SaunanNimi = model.SaunanNimi;
        //    //view.SaunanTila = true;
        //    //view.SaunanNykyLampotila = model.Sauna.SaunanNykyLampotila;


        //    ////view.TaloLampoID = model.TaloLampo.TaloLampoID;
        //    //view.HuoneenNimi = model.TaloLampo.HuoneenNimi;
        //    //view.TalonTavoiteLampotila = model.TaloLampo.TalonTavoiteLampotila;
        //    //view.TalonNykyLampotila = model.TaloLampo.TalonNykyLampotila;


        //    ////view.ValoID = model.Valo.ValoID;
        //    //view.ValonNimi = model.Valo.ValonNimi;
        //    //view.ValonTila = model.Valo.ValonTila;
        //    //view.ValonMaara = model.Valo.ValonMaara;

        //    //ViewBag.TalonNimi = new SelectList((from ta in db.Talo
        //    //                                    select new
        //    //                                    { TaloID = ta.TaloID, TalonNimi = ta.TalonNimi })
        //    //, "TaloID", "TalonNimi", view.TaloID);

        //    db.SaveChanges();

        //    return RedirectToAction("Index");
        //}
        //public ActionResult TalonTiedotIndex(int? id, int? saunaID,int? taloLampoID, int? valoID)
        //{
        //    var viewModel = new TalonTiedotViewModel();
        //    viewModel.Talot = db.Talo
        //        .Include(i => i.TalonNimi)
        //        .Include(i => i.Valo.Select(c => c.ValoID))
        //        .Include(i => i.Valo.Select(c => c.ValonNimi))
        //        .Include(i => i.Valo.Select(c => c.ValonTila))
        //        .Include(i => i.Valo.Select(c => c.ValonMaara));
        //        //.OrderBy(i => i.TalonNimi);



        //    if (id != null)
        //    {
        //        ViewBag.TaloID = id.Value;
        //        viewModel.Talot = viewModel.Talot.Where(
        //            i => i.TaloID == id.Value);
        //    }

        //    if (valoID != null)
        //    {
        //        ViewBag.ValoID = valoID.Value;
        //        viewModel.Valot = viewModel.Valot.Where(
        //            x => x.ValoID == valoID);
        //    }

        //    return View(viewModel);
        //}
        //public ActionResult TalonTiedotIndex( int? id, int? iidii)
        //{
        //    var viewModel = new TalonTiedotViewModel();
        //    viewModel.Talot = db.Talot
        //        .Include(c => c.Saunat.Select(i => i.TaloID))
        //        .Include(c => c.TaloLampot.Select(i => i.TaloID))
        //        .Include(c => c.Valo)
        //        .Include(c=> c.TaloID);
        //    //var iidii = db.Talo.Find(model.Talot.Select(i=>i.TaloID));







        //    if (id != null)
        //    {
        //        ViewBag.TaloID = id.Value;
        //        viewModel.Valot = viewModel.Talot.Where(
        //            i => i.TaloID == id.Value).Single().Valot;
        //    }

        //    if (iidii != null)
        //    {
        //        ViewBag.ValoID = courseID.Value;
        //        viewModel.Enrollments = viewModel.Courses.Where(
        //            x => x.CourseID == courseID).Single().Enrollments;
        //    }




        //    if (id != null)
        //    {
        //        ViewBag.InstructorID = id.Value;
        //        viewModel.Courses = viewModel.Instructors.Where(
        //            i => i.ID == id.Value).Single().Courses;
        //    }

        //    if (courseID != null)
        //    {
        //        ViewBag.CourseID = courseID.Value;
        //        viewModel.Enrollments = viewModel.Courses.Where(
        //            x => x.CourseID == courseID).Single().Enrollments;
        //    }


        //    return View(viewModel);
        //}

        //    public ActionResult Timo1(int? id)
        //{
        //    using (var context = new ÄlytaloToinenEntities())
        //    {
        //        var tiedot = from b in context.Talot
        //                     where b.TaloID == id
        //                     where b.Valo.TaloID==id
        //                     where b.Sauna.TaloID == id
        //                     where b.TaloLampo.TaloID == id
        //                     select b;
        //    }

        //}


        //List<Valo> valotx = dbvaloent.Valot.ToList();

        //var modelx = (from v in dbvaloent.Valot
        //             where v.TaloID == id
        //             select new
        //             {
        //                 ValoID = v.ValoID,
        //                 ValonNimi = v.ValonNimi,
        //                 ValonMaara = v.ValonMaara,
        //                 ValonTila = v.ValonTila
        //             }).FirstOrDefault();



        //view.TaloID = talo.Valo?.TaloID;
        //view.ValoID = talo.Valo?.ValoID;
        //view.ValonNimi = talo.Valo?.ValonNimi;
        //view.ValonMaara = talo.Valo?.ValonMaara;
        //view.ValonTila = talo.Valo?.ValonTila;

        ////view.TaloID = talo.TaloID;
        //view.SaunanNimi = talo.Sauna?.SaunanNimi;
        //view.SaunanNykyLampotila = talo.Sauna?.SaunanNykyLampotila;
        //view.SaunanTila = talo.Sauna?.SaunanTila;


        ////view.TaloID = talo.TaloID;
        //view.HuoneenNimi = talo.TaloLampo?.HuoneenNimi;
        //view.TalonNykyLampotila = talo.TaloLampo?.TalonNykyLampotila;
        //view.TalonTavoiteLampotila = talo.TaloLampo?.TalonTavoiteLampotila;
        //model.Add(view);

        //if (talo.Valo?.TaloID != null)
        //{
        //    //List<Valo> valot = entities.Valot.ToList();
        //    foreach (Talo t in talot)
        //    {
        //        TalonTiedotViewModel view = new TalonTiedotViewModel();
        //        view.TaloID = talo.Valo.TaloID;
        //        view.ValoID = talo.Valo?.ValoID;
        //        view.ValonNimi = talo.Valo?.ValonNimi;
        //        view.ValonMaara = talo.Valo?.ValonMaara;
        //        view.ValonTila = talo.Valo?.ValonTila;
        //        model.Add(view);
        //    }
        //}


        //var talot = from m in db.Talot
        //             select m;
        //var valot = from m in db.Valot
        //            select m;
        //var saunat = from m in db.Saunat
        //            select m;
        //var talolampot = from m in db.TaloLampot
        //            select m;


        //if (id!=null)
        //{
        //    talot = talot.Where(s => s.TaloID==id);
        //    if(==id))
        //        {

        //    }
        //}

        //return View(talot);

        //List<TalonTiedotViewModel> model = new List<TalonTiedotViewModel>();
        //ÄlytaloToinenEntities entities = new ÄlytaloToinenEntities();


        //try
        //{
        //    //var hoo = entities.Talot.Include(c => c.Valot).Single(t => t.TaloID == id);
        //    List<Talo> talot = entities.Talot.Include(c=>c.Valo).ToList();
        //    foreach (Talo talo in talot)
        //    {
        //        TalonTiedotViewModel view = new TalonTiedotViewModel();
        //        view.TaloID = talo.TaloID;
        //        view.TalonNimi = talo.TalonNimi;
        //        view.ValoID = talo.Valo?.ValoID;
        //        view.ValonNimi = talo.Valo?.ValonNimi;
        //        view.TaloID = talo.Valo?.TaloID;
        //        model.Add(view);
        //    }
        //}
        //finally
        //{
        //    entities.Dispose();
        //}
        //return View(model);
        //var vm = new TalonTiedotViewModel();
        //vm.Talot = db.Talot.Where(x=>x.TaloID==id).ToList();
        //vm.Valot = db.Valot.Where(x=>x.TaloID==id).ToList();
        //vm.Saunat = db.Saunat.Where(x => x.TaloID == id).ToList();
        //vm.TaloLampot = db.TaloLampot.Where(x => x.TaloID == id).ToList();
        //return View(vm);

        //TalonTiedotViewModel view = new TalonTiedotViewModel();
        //Talo talo = db.Talot.Find(id);

        //ÄlytaloToinenEntities entities = new ÄlytaloToinenEntities();
        //List<TalonTiedotViewModel> model = new List<TalonTiedotViewModel>();

        //try
        //{
        //    //var lista = db.Talot.Where(c => c.TaloID == id)
        //    //    .Include(c => c.Valo.TaloID == id)
        //    //    .Include(c => c.Sauna.TaloID == id)
        //    //    .Include(c => c.TaloLampo.TaloID == id); /*.Include(db.Valot.Where(c=>c.TaloID==id));*/

        //    var malli = new TalonTiedotViewModel()
        //    {

        //        // You'll likely want a .ToList() after these to ensure things work as expected
        //        //Talot = db.Talot.Select(x => x.TaloID==id),
        //        Valot =from x in entities.Valot
        //               where x.TaloID==id
        //               select x,
        //        Saunat = from x in entities.Saunat
        //                 where x.TaloID == id
        //                 select x,
        //        TaloLampot = from x in entities.TaloLampot
        //                     where x.TaloID == id
        //                     select x

        //    }
        //    malli.ToList();



        //var tiedot = from b in entities.Talot
        //                 //where b.TaloID == id
        //                 where b.TaloID == id
        //                 //where b.Sauna.TaloID == id
        //                 //where b.TaloLampo.TaloID == id
        //                 select b;
        //    List<Valo> talot = tiedot.ToList();



        //List<Talo> talot = tiedot.ToList();

        //List<Talo> talot = lista.ToList();

        // var incomeList = db.Talot
        //    .Where(c => c.TaloID == id)
        //    .Include(i => i.Valo)
        //    .Include(i => i.Sauna)
        //    .Include(i => i.TaloLampo)
        //    .Include(i => i.ListPeriods.Select(lp => lp.PeriodType))
        //    .Select(i => new
        //    {
        //        CompanyTitle = i.CompanyId + "/" + i.Company.CompanyName,
        //        PeriodTypeNames = i.ListPeriods.Select(lp => lp.PeriodType.PeriodTypeName)
        //    })
        //.ToList();

        //List<Talo> talot = entities.Talot.ToList();
        //    foreach (Valo osaaminen in talot)
        //    {
        //        TalonTiedotViewModel view = new TalonTiedotViewModel();

        //        view.ValoID = osaaminen.ValoID;
        //        view.ValonNimi = osaaminen.ValonNimi;
        //        //view.OpettamisenHalukkuus = osaaminen.OpettamisenHalukkuus;
        //        //view.Kuvaus = osaaminen.Kuvaus;
        //        //view.OpettajaID = osaaminen.Opettaja.OpettajaID;
        //        //view.Etunimi = osaaminen.Opettaja.Etunimi;
        //        //view.Sukunimi = osaaminen.Opettaja.Sukunimi;
        //        //view.Sähköposti = osaaminen.Opettaja.Sähköposti;
        //        //view.Henkilönumero = osaaminen.Opettaja.Henkilönumero;
        //        //view.Yksikkö = osaaminen.Opettaja.Yksikkö;
        //        //view.Toimenkuva = osaaminen.Opettaja.Toimenkuva;
        //        //view.Nimi = osaaminen.Opettaja.Etunimi + " " + osaaminen.Opettaja.Sukunimi;
        //        model.Add(view);
        //    }

        //}
        //finally
        //{
        //    entities.Dispose();
        //}
        //return View(model);


        //if (id == null)
        //{
        //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //}

        //if (talo == null)
        //{
        //    return HttpNotFound();
        //}

        //view.TaloID = talo.TaloID
        //    & talo.Valo?.TaloID
        //    & talo.Sauna?.TaloID
        //    & talo.TaloLampo?.TaloID;

        //view.TalonNimi = talo.TalonNimi;


        ////view.TaloID = talo.Valo.TaloID;
        //view.ValoID = talo.Valo?.ValoID;
        //view.ValonNimi = talo.Valo?.ValonNimi;
        //view.ValonMaara = talo.Valo?.ValonMaara;
        //view.ValonTila = talo.Valo?.ValonTila;


        //view.TaloID = talo.TaloID;
        //view.SaunanNimi = talo.Sauna?.SaunanNimi;
        //view.SaunanNykyLampotila = talo.Sauna?.SaunanNykyLampotila;
        //view.SaunanTila = talo.Sauna?.SaunanTila;


        //view.TaloID = talo.TaloID;
        //view.HuoneenNimi = talo.TaloLampo?.HuoneenNimi;
        //view.TalonNykyLampotila = talo.TaloLampo?.TalonNykyLampotila;
        //view.TalonTavoiteLampotila = talo.TaloLampo?.TalonTavoiteLampotila;



        ////var tiedot = (db.Valot.Where(e => e.TaloID == ide)
        ////        , db.Saunat.Where(e => e.TaloID == ide)
        ////        , db.TaloLampot.Where(e => e.TaloID == ide));

        //return View(view);

        //List<OpettajaOsaaminen> model = new List<OpettajaOsaaminen>();
        //OpettajakantaEntities entities = new OpettajakantaEntities();
        //try
        //{
        //    List<Osaamiset> osaamiset = entities.Osaamiset.ToList();
        //    foreach (Osaamiset osaaminen in osaamiset)
        //{
        //    OpettajaOsaaminen view = new OpettajaOsaaminen();
        //    view.OsaamisID = osaaminen.OsaamisID;
        //    view.Osaaminen = osaaminen.Osaaminen;
        //    view.OpettamisenHalukkuus = osaaminen.OpettamisenHalukkuus;
        //    view.Kuvaus = osaaminen.Kuvaus;
        //    view.OpettajaID = osaaminen.Opettaja.OpettajaID;
        //    view.Etunimi = osaaminen.Opettaja.Etunimi;
        //    view.Sukunimi = osaaminen.Opettaja.Sukunimi;
        //    view.Sähköposti = osaaminen.Opettaja.Sähköposti;
        //    view.Henkilönumero = osaaminen.Opettaja.Henkilönumero;
        //    view.Yksikkö = osaaminen.Opettaja.Yksikkö;
        //    view.Toimenkuva = osaaminen.Opettaja.Toimenkuva;
        //    view.Nimi = osaaminen.Opettaja.Etunimi + " " + osaaminen.Opettaja.Sukunimi;
        //    model.Add(view);
        //}

    }
            //finally
            //{
            //    entities.Dispose();
            //}
            //return View(model);
        
        
    
}
