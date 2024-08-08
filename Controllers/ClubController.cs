using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interface;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;

        public ClubController( IClubRepository clubRepository, IPhotoService photoService)
        {
            _clubRepository = clubRepository;
            _photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubRepository.GetAllClubsAsync();  
            return View(clubs);
        }
        
        public async Task<IActionResult> Detail(String id)
        {
            Club club = await _clubRepository.GetClubByIdAsync(id);
            return View(club);
        }
        public async Task<IActionResult> Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel club)
        {
            if(ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(club.Image);

                var clubModel = new Club
                {
                    Title = club.Title,
                    Description = club.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                       City = club.Address.City,
                       Street = club.Address.Street,
                       State = club.Address.State,
                    }
                };
                _clubRepository.Add(clubModel);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }
            return View(club);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var club = await _clubRepository.GetClubByIdAsync(id);
            if (club == null) return View("Error");
            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                ClubCategory = club.ClubCategory,
                URL = club.Image,
                AddressId = club.AddressId,
                Address = club.Address,
            };
            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, EditClubViewModel editClubViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit",editClubViewModel);
            }
            var clubModel = await _clubRepository.GetClubByIdAsyncNoTracking(id.ToString());
            if (clubModel == null)
            {
                return View("Error");
            }
            var photoResult = await _photoService.AddPhotoAsync(editClubViewModel.Image);
            if(photoResult == null)
            {
                ModelState.AddModelError("Image", "Photo upload failed");
                return View(editClubViewModel);
            }
            if(!string.IsNullOrEmpty(clubModel.Image))
            {
                _ = _photoService.DeletePhotoAsync(clubModel.Image);
            }
            var club = new Club
            {
                Id = id,
                Title = editClubViewModel.Title,
                Description = editClubViewModel?.Description,
                Image = photoResult.Url.ToString(),
                ClubCategory = editClubViewModel.ClubCategory,
                AddressId = editClubViewModel.AddressId,
                Address = editClubViewModel.Address

            };
            _clubRepository.Update(club);
            return RedirectToAction("Index");   
        }

        public async Task<IActionResult> Delete(string id)
        {
            var clubDetails = await _clubRepository.GetClubByIdAsync(id);
            if(clubDetails == null) return View("Error");
            return View(clubDetails);

        }

        [HttpPost, ActionName("Delete")]  
        public async Task<IActionResult> DeleteClub(string id)
        {
            var clubDetails = await _clubRepository.GetClubByIdAsync(id);
            if (clubDetails == null)
            {
                return View("Error");
            }
            _clubRepository.Delete(clubDetails);
            return RedirectToAction("Index");   

        }
    }
}
