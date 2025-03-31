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

        public async Task<(IEnumerable<AssetViewModel>, int)> FilterAssetsAsync(int? categoryId, int? statusId, int? locationId, string? searchTerm, int page, int pageSize)
        {
            var (assets, totalItems) = await _assetRepository.FilterByAsync(categoryId, statusId, locationId, searchTerm, page, pageSize);
            return (assets.Select(MapToViewModel), totalItems);
        }


        public async Task AddAssetAsync(AssetViewModel assetViewModel)
        {
            // Chuyển đổi ViewModel sang Model
            var asset = MapToModel(assetViewModel);

            // Kiểm tra xem AssetCode đã tồn tại chưa
            if (await _assetRepository.ExistsAsync(asset.AssetCode))
            {
                throw new InvalidOperationException($"Tài sản với mã '{asset.AssetCode}' đã tồn tại. Hãy sử dụng mã khác.");
            }

            // Xử lý upload ảnh nếu có file được chọn
            if (assetViewModel.ImageFile != null && assetViewModel.ImageFile.Length > 0)
            {
                try
                {
                    // Tạo thư mục lưu ảnh nếu chưa có
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder);

                    // Lấy tên file an toàn và tạo tên file duy nhất
                    string fileName = Path.GetFileName(assetViewModel.ImageFile.FileName);
                    string uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Lưu file lên server
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await assetViewModel.ImageFile.CopyToAsync(fileStream);
                    }

                    // Lưu đường dẫn ảnh vào model
                    asset.ImageUrl = "/uploads/" + uniqueFileName;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi lưu ảnh: {ex.Message}");
                    asset.ImageUrl = "/uploads/default.png"; // Sử dụng ảnh mặc định nếu có lỗi
                }
            }
            else
            {
                // Nếu không upload ảnh, gán ảnh mặc định
                asset.ImageUrl = "/uploads/default.png";
            }

            // Lưu đối tượng asset vào database qua repository
            await _assetRepository.AddAsync(asset);
        }
        public async Task UpdateAssetAsync(AssetViewModel assetViewModel)
        {
            var existingAsset = await _assetRepository.GetByIdAsync(assetViewModel.AssetId);
            if (existingAsset == null)
            {
                throw new InvalidOperationException("Tài sản không tồn tại.");
            }

            // Kiểm tra trùng AssetCode: Nếu AssetCode mới khác với AssetCode hiện tại,
            // kiểm tra xem AssetCode mới có tồn tại ở tài sản khác không.
            if (!string.Equals(existingAsset.AssetCode, assetViewModel.AssetCode, StringComparison.OrdinalIgnoreCase))
            {
                bool duplicateExists = await _assetRepository.ExistsAsync(assetViewModel.AssetCode);
                if (duplicateExists)
                {
                    throw new InvalidOperationException($"Asset với mã '{assetViewModel.AssetCode}' đã tồn tại.");
                }
            }

            string? existingImagePath = existingAsset.ImageUrl;

            if (assetViewModel.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(assetViewModel.ImageFile.FileName)}";
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

        public async Task<Asset> GetAssetByCodeAsync(string assetCode)
        {
            return await _assetRepository.GetAssetByCodeAsync(assetCode);
        }
    }
}
