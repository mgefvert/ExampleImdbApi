using ImdbApi.GenericControllers;
using ImdbApi.Logging;
using ImdbData.Entities;
using ImdbData.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImdbApi.Controllers;

[ApiController]
[Route("imdb/title-basic")]
public class ImdbTitleBasicController : AbstractCrudController<int, DbTitleBasic, TitleBasicQuery, ITitleBasicService>
{
    public ImdbTitleBasicController(
        ITitleBasicService titleBasicService,
        ApiLogRequest log
    ) 
        : base(titleBasicService, log)
    {
    }

    protected override string ObjectToString(DbTitleBasic item) => item.ToString();
}