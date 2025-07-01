using ChurchFlowAPI.Data;
using ChurchFlowAPI.DTOs;
using ChurchFlowAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ChurchFlowAPI.Services
{
    //interface
    public interface IDocumentService
    {
        Task<IEnumerable<ReturnDocumentDto>> GetAllAsync();
        Task<ReturnDocumentDto> UploadAsync(UploadDocumentDto dto, string uploadedById);
        Task<ReturnDocumentDto> GetByIdAsync(int id);
    }

    //logic
    public class DocumentService : IDocumentService
    {
        private readonly ApplicationDbContext _context;

        public DocumentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReturnDocumentDto>> GetAllAsync()
        {
            var documents = await _context.Documents
                .Include(d => d.UploadedBy)
                .ToListAsync();

            return documents.Select(d => new ReturnDocumentDto
            {
                Id = d.Id,
                Title = d.Title,
                Description = d.Description,
                FilePath = d.FilePath,
                UploadedAt = d.UploadedAt,
                UploadedByName = d.UploadedBy?.FullName
            });
        }

        public async Task<ReturnDocumentDto> UploadAsync(UploadDocumentDto dto, string uploadedById)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = Path.GetFileNameWithoutExtension(dto.File.FileName);
            var extension = Path.GetExtension(dto.File.FileName);
            var newFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            var relativePath = $"/uploads/{newFileName}";

            var document = new Document
            {
                Title = dto.Title,
                Description = dto.Description,
                FilePath = $"/uploads/{newFileName}",
                UploadedAt = DateTime.UtcNow,
                UploadedById = uploadedById // extract from token later
            };

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return new ReturnDocumentDto
            {
                Id = document.Id,
                Title = document.Title,
                Description = document.Description,
                FilePath = document.FilePath,
                UploadedAt = document.UploadedAt,
                //UploadedByName = (await _context.Users.FindAsync(dto.UploadedById))?.UserName
            };
        }

        public async Task<ReturnDocumentDto> GetByIdAsync(int id)
        {
            var d = await _context.Documents
                .Include(x => x.UploadedBy)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (d == null) return null;

            return new ReturnDocumentDto
            {
                Id = d.Id,
                Title = d.Title,
                Description = d.Description,
                FilePath = d.FilePath,
                UploadedAt = d.UploadedAt,
                UploadedByName = d.UploadedBy?.FullName
            };
        }
    }


}
