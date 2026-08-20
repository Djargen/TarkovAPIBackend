using System;
using System.Collections.Generic;
using System.Text;
using Tarkov.Infrastructure.DTOResponses;
using Tarkov.Infrastructure.Data;
using Tarkov.Infrastructure.Data.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Tarkov.API.Services
{
    internal class TarkovSyncService(TarkovDbContext context, TarkovAPIService apiService, ILogger<TarkovSyncService> logger)
    {
        public async Task SyncDataAsync()
        {
            try
            {
                logger.LogInformation("Starting synchronisation with Tarkov.dev API....");
                var remoteItems = await apiService.GetTarkovItemsAsync();

                if (remoteItems != null)
                {
                    foreach (var dto in remoteItems.Values)
                    {
                        var existingItem = await context.Items.FindAsync(dto.Id);

                        if (existingItem == null)
                        {
                            var newItem = new ItemEntity
                            {
                                Id = dto.Id,
                                Name = dto.Name,
                                Image = dto.Image,
                                FleaLowPrice = dto.FleaLowPrice,
                                BestTraderName = dto.BestTraderName,
                                BestTraderPrice = dto.BestTraderPrice
                            };
                            context.Items.Add(newItem);
                        }
                        else
                        {
                            existingItem.Name = dto.Name;
                            existingItem.Image = dto.Image;
                            existingItem.FleaLowPrice = dto.FleaLowPrice;
                            existingItem.BestTraderName = dto.BestTraderName;
                            existingItem.BestTraderPrice = dto.BestTraderPrice;
                        }
                    }

                    await context.SaveChangesAsync();
                    logger.LogInformation("Succesfully synchronized database with Tarkov.dev API.");
                }
            }
            catch 
            {
                logger.LogWarning("Can't esttablish connection with Tarkov.dev API, API now uses local database.");
            }
        }
    }
}