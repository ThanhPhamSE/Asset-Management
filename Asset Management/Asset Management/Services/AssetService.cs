using Asset_Management.Models;
using Asset_Management.Repositories.IRepositories;
using Asset_Management.Services.IServices;
using Asset_Management.ViewModels;
using System;

namespace Asset_Management.Services
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IWebHostEnvironment _environment;

        public AssetService(IAssetRepository assetRepository, IWebHostEnvironment environment)
        {
            _assetRepository = assetRepository;
            _environment = environment;
        }

        public async Task<IEnumerable<AssetViewModel>> GetAllAssetsAsync()
        {
            var assets = await _assetRepository.GetAllAsync();
            return assets.Select(MapToViewModel);
        }

        public async Task<AssetViewModel?> GetAssetByIdAsync(int id)
        {
            var asset = await _assetRepository.GetByIdAsync(id);
            return asset != null ? MapToViewModel(asset) : null;
        }

        public async Task<IEnumerable<AssetViewModel>> FilterAssetsAsync(int? categoryId, int? statusId, int? locationId)
        {
            var assets = await _assetRepository.FilterByAsync(categoryId, statusId, locationId);
            return assets.Select(MapToViewModel);
        }

        public async Task AddAssetAsync(AssetViewModel assetViewModel)
        {
            var asset = MapToModel(assetViewModel);

            if (assetViewModel.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = $"{Guid.NewGuid()}_{assetViewModel.ImageFile.FileName}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await assetViewModel.ImageFile.CopyToAsync(fileStream);
                }

                asset.ImageUrl = "/uploads/" + uniqueFileName;
            }

            await _assetRepository.AddAsync(asset);
        }

        //public async Task UpdateAssetAsync(AssetViewModel assetViewModel)
        //{


        //    // Xử lý ảnh
        //    if (assetViewModel.ImageFile != null)
        //    {
        //        string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
        //        Directory.CreateDirectory(uploadsFolder);

        //        string uniqueFileName = $"{Guid.NewGuid()}_{assetViewModel.ImageFile.FileName}";
        //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await assetViewModel.ImageFile.CopyToAsync(fileStream);
        //        }

        //        // Xóa ảnh cũ nếu có
        //        if (!string.IsNullOrEmpty(assetViewModel.ImagePath))
        //        {
        //            string oldFilePath = Path.Combine(_environment.WebRootPath, assetViewModel.ImagePath.TrimStart('/'));
        //            if (System.IO.File.Exists(oldFilePath))
        //            {
        //                System.IO.File.Delete(oldFilePath);
        //            }
        //        }

        //        assetViewModel.ImagePath = "/uploads/" + uniqueFileName;
        //    }


        //    var asset = MapToModel(assetViewModel);
        //    await _assetRepository.UpdateAsync(asset);
        //}

        public async Task UpdateAssetAsync(AssetViewModel assetViewModel)
        {
            var existingAsset = await _assetRepository.GetByIdAsync(assetViewModel.AssetId);
            if (existingAsset == null)
            {
                throw new InvalidOperationException("Tài sản không tồn tại.");
            }

            string? existingImagePath = existingAsset.ImageUrl;

            if (assetViewModel.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = $"{Guid.NewGuid()}_{assetViewModel.ImageFile.FileName}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await assetViewModel.ImageFile.CopyToAsync(fileStream);
                }

                // Xóa ảnh cũ nếu có
                if (!string.IsNullOrEmpty(existingImagePath))
                {
                    string oldFilePath = Path.Combine(_environment.WebRootPath, existingImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                assetViewModel.ImagePath = "/uploads/" + uniqueFileName;
            }
            else
            {
                assetViewModel.ImagePath = existingImagePath;
            }

            var updatedAsset = MapToModel(existingAsset, assetViewModel);
            await _assetRepository.UpdateAsync(updatedAsset);
        }


        public async Task DeleteAssetAsync(int id)
        {
            await _assetRepository.DeleteAsync(id);
        }

        private static AssetViewModel MapToViewModel(Asset asset)
        {
            return new AssetViewModel
            {
                AssetId = asset.AssetId,
                AssetCode = asset.AssetCode,
                AssetName = asset.AssetName,
                CategoryId = asset.CategoryId,
                PurchaseDate = asset.PurchaseDate,
                PurchasePrice = asset.PurchasePrice,
                CurrentValue = asset.CurrentValue,
                DepreciationRate = asset.DepreciationRate,
                StatusId = asset.StatusId,
                ImagePath = asset.ImageUrl,
                LocationId = asset.LocationId,

                // Lấy tên từ navigation properties
                CategoryName = asset.Category?.CategoryName,
                LocationName = asset.Location?.LocationName,
                StatusName = asset.Status?.StatusName
            };
        }

        private static Asset MapToModel(AssetViewModel assetViewModel)
        {
            return new Asset
            {
                AssetId = assetViewModel.AssetId,
                AssetCode = assetViewModel.AssetCode,
                AssetName = assetViewModel.AssetName,
                CategoryId = assetViewModel.CategoryId,
                PurchaseDate = assetViewModel.PurchaseDate,
                PurchasePrice = assetViewModel.PurchasePrice,
                CurrentValue = assetViewModel.CurrentValue,
                DepreciationRate = assetViewModel.DepreciationRate,
                StatusId = assetViewModel.StatusId,
                ImageUrl = assetViewModel.ImagePath,
                LocationId = assetViewModel.LocationId
            };
        }

        private static Asset MapToModel(Asset existingAsset, AssetViewModel assetViewModel)
        {
            existingAsset.AssetCode = assetViewModel.AssetCode;
            existingAsset.AssetName = assetViewModel.AssetName;
            existingAsset.CategoryId = assetViewModel.CategoryId;
            existingAsset.PurchaseDate = assetViewModel.PurchaseDate;
            existingAsset.PurchasePrice = assetViewModel.PurchasePrice;
            existingAsset.CurrentValue = assetViewModel.CurrentValue;
            existingAsset.DepreciationRate = assetViewModel.DepreciationRate;
            existingAsset.StatusId = assetViewModel.StatusId;
            existingAsset.ImageUrl = assetViewModel.ImagePath;
            existingAsset.LocationId = assetViewModel.LocationId;

            return existingAsset; // Trả về đối tượng đã cập nhật
        }

        public async Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync()
        {
            var categories = await _assetRepository.GetCategoriesAsync() ?? new List<Category>();
            return categories.Select(c => new CategoryViewModel { CategoryId = c.CategoryId, CategoryName = c.CategoryName });
        }

        public async Task<IEnumerable<StatusViewModel>> GetStatusesAsync()
        {
            var statuses = await _assetRepository.GetStatusesAsync() ?? new List<Status>();
            return statuses.Select(s => new StatusViewModel { StatusId = s.StatusId, StatusName = s.StatusName });
        }

        public async Task<IEnumerable<LocationViewModel>> GetLocationsAsync()
        {
            var locations = await _assetRepository.GetLocationsAsync() ?? new List<Location>();
            return locations.Select(l => new LocationViewModel { LocationId = l.LocationId, LocationName = l.LocationName });
        }

    }
}
