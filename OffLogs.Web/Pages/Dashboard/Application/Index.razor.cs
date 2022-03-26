using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Application;
using OffLogs.Web.Resources;
using OffLogs.Web.Services;
using OffLogs.Web.Services.Http;

namespace OffLogs.Web.Pages.Dashboard.Application;

public partial class Index
{
    [Inject]
    private IApiService ApiService { get; set; }

    [Inject]
    private ToastService ToastService { get; set; }
    
    private List<ApplicationListItemDto> _applications = new();
    private int _currentPage = 1;
    private long? _selectedApplicationId;
    private ApplicationListItemDto _applicationForDeletion = null;
    private ApplicationListItemDto _applicationForSharing = null;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await LoadListAsync(false);
    }

    private async Task LoadListAsync(bool isLoadNextPage = true)
    {
        if (isLoadNextPage)
        {
            _currentPage++;
        }
        var response = await ApiService.GetApplicationsAsync(new GetListRequest()
        {
            Page = _currentPage
        });
        _applications = response.Items.ToList();
        StateHasChanged();
    }

    private async Task OnClickRowAsync(ApplicationListItemDto application)
    {
        _selectedApplicationId = application.Id;
        await Task.CompletedTask;
    }

    private void OnCloseInfoModal()
    {
        _selectedApplicationId = null;
    }

    private Task OnApplicationAdded(ApplicationDto application)
    {
        _applications.Add(new ApplicationListItemDto() {
            Id = application.Id,
            CreateTime = application.CreateTime,
            Name = application.Name,
            UserId = application.UserId
        });
        return Task.CompletedTask;
    }

    private async Task OnDeleteAppAsync()
    {
        _applicationForDeletion = _applicationForDeletion 
                                  ?? throw new ArgumentNullException(nameof(_applicationForDeletion));
        try
        {
            var applicationId = _applicationForDeletion.Id;
            _applicationForDeletion = null;
            ToastService.AddInfoMessage("Application deleting...");
            await ApiService.DeleteApplicationAsync(applicationId);
            ToastService.AddInfoMessage("Application is deleted");
            _applications = _applications.Where(i => i.Id != applicationId).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            ToastService.AddErrorMessage(CommonResources.Error_ApplicationDeletionError);
        }
        finally
        {
            StateHasChanged();
        }
    }
}
