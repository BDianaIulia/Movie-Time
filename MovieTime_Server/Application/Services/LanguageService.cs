using Application.Interfaces;
using Domain.Core;
using Domain.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class LanguageService: ILanguageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LanguageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Language> GetLanguageByCode(string code)
        {
            return await _unitOfWork.Languages.GetLanguageByCode(code);
        }

        public async Task<Language> RegisterLanguage(LanguageDto language)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    Language existentLanguage = await GetLanguageByCode(language.Code);
                    if (existentLanguage != null)
                        return await Task.FromResult(existentLanguage);

                    var languageId = Guid.NewGuid();
                    var isTransactionSucceeded = true;
                    var langugageToRegister = new Language
                    {
                        Id = languageId,
                        Name = language.Name,
                        Code = language.Code
                    };
                    var isLangugageRegistered = await _unitOfWork.Languages.Register(langugageToRegister);
                    if (isLangugageRegistered == 0)
                    {
                        isTransactionSucceeded = false;
                    }
                    if (isTransactionSucceeded)
                    {
                        transaction.Commit();
                        return await Task.FromResult(langugageToRegister);
                    }
                    return await Task.FromResult<Language>(null);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return await Task.FromResult<Language>(null);
                }
            }
        }
    }
}
