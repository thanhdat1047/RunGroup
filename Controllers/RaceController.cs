using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interface;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _raceRepository;
        private readonly IPhotoService _photoService;

        public RaceController(IRaceRepository raceRepository, IPhotoService photoService)
        {
            _raceRepository = raceRepository;
            _photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races = await _raceRepository.GetAllRacesAsync();
            return View(races);
        }
        public async Task<IActionResult> Detail (string id)
        {
            Race race = await _raceRepository.GetRaceByIdAsync(id); 
            return View(race);

        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceModel race)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(race.Image);
                var raceModel = new Race
                {
                    Title = race.Title,
                    Description = race.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        City = race.Address.City,
                        State = race.Address.State,
                        Street = race.Address.Street,
                    }
                };
                _raceRepository.Add(raceModel);
                return RedirectToAction("Index");
            } else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }
            return View(race);  
        }

        public async Task<IActionResult> Edit(string id)
        {
            var race = await _raceRepository.GetRaceByIdAsync(id);
            if(race == null) return View("Error");

            var raceVM = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                URL = race.Image,
                Address = race.Address,
                AddressId = race.AddressId,
                RaceCategory = race.RaceCategory
            };
            return View(raceVM);
                
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, EditRaceViewModel editRaceViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit race");
                return View("Edit", editRaceViewModel);
            }
            var raceModel = await _raceRepository.GetRaceByIdAsyncNoTracking(id.ToString());
            if (raceModel == null)
            {
                return View("Error");
            }
            var photoResult = await _photoService.AddPhotoAsync(editRaceViewModel.Image);
            if (photoResult == null)
            {
                ModelState.AddModelError("Image", "Photo upload failed");
                return View(editRaceViewModel);
            }
            if (!string.IsNullOrEmpty(raceModel.Image))
            {
                _ = _photoService.DeletePhotoAsync(raceModel.Image);
            }

            var race = new Race
            {
                Id = id,
                Title = editRaceViewModel.Title,
                Description = editRaceViewModel.Description,
                Image = photoResult.Url.ToString(),
                RaceCategory = editRaceViewModel.RaceCategory,
                Address = editRaceViewModel.Address,
                AddressId = editRaceViewModel.AddressId,
            };
            _raceRepository.Update(race);
            return RedirectToAction("Index");


        }

        public async Task<IActionResult> Delete(string id)
        {
            var race = await _raceRepository.GetRaceByIdAsync(id);
            if (race == null) return View("Error");
            return View(race);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(string id)
        {
            var race = await _raceRepository.GetRaceByIdAsync(id);
            if (race == null) return View("Error");
            _raceRepository.Delete(race);
            return RedirectToAction("Index");
        }
    }
}
