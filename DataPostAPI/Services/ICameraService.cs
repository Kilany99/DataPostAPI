using DataPostAPI.Helpers;
using DataPostAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataPostAPI.Services
{
    public interface ICameraService
    {
        IEnumerable<Camera> GetAll();
        Camera GetById(int id);
        Camera Create(Camera camera);
        void Update(Camera camera);
        void Delete(int id);
    }

    public class CameraService : ICameraService
    {
        private ClientContext _context;

        public CameraService(ClientContext context)
        {
            _context = context;
        }

   

        public IEnumerable<Camera> GetAll()
        {
            return _context.cameras;
        }

        public Camera GetById(int id)
        {
            return _context.cameras.Find(id);
        }

        public Camera Create(Camera camera)
        {
           
            if (string.IsNullOrWhiteSpace(camera.ZoneDescription))
                throw new AppException("Zone details is required");

            if (string.IsNullOrWhiteSpace(camera.ZonePriority.ToString()))
                throw new AppException("Zone priority is required");

            if (_context.cameras.Any(x => x.ZoneDescription == camera.ZoneDescription))
                throw new AppException("Zone \"" + camera.ZoneDescription + "\" is already taken");


            _context.cameras.Add(camera);
            _context.SaveChanges();

            return camera;
        }

        public void Update(Camera userParam)
        {
            var camera = _context.cameras.Find(userParam.CameraZoneid);

            if (camera == null)
                throw new AppException("zone not found");

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.ZonePriority.ToString()) && userParam.ZoneDescription != camera.ZoneDescription)
            {
                // throw error if the new zone details is already taken
                if (_context.cameras.Any(x => x.ZoneDescription == userParam.ZoneDescription))
                    throw new AppException("Zone " + userParam.ZoneDescription + " is already taken");

                camera.ZoneDescription = userParam.ZoneDescription;
            }

            // update zone properties if provided

            if (!string.IsNullOrWhiteSpace(userParam.ZonePriority.ToString()))
                camera.ZonePriority = userParam.ZonePriority;

           
            _context.cameras.Update(camera);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var camera = _context.cameras.Find(id);
            if (camera != null)
            {
                _context.cameras.Remove(camera);
                _context.SaveChanges();
            }
        }

        
    }
}
