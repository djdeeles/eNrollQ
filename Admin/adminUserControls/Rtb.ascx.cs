using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using eNroll.App_Data;

public partial class adminUserControls_Rtb : UserControl
{
    private readonly Entities _entities = new Entities();
    protected List<Forms> FormList = null;
    protected List<Lists> LList = null;
    protected List<Def_photoAlbum> PAlbumsList = null;
    protected List<Def_photoAlbumCategory> PCategoriesList = null;
    protected List<Rss> RssList = null;
    protected List<VideoCategories> VCategoriesList = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        RadEditor1.AutoResizeHeight = false;
        
        RadEditor1.DisableFilter(Telerik.Web.UI.EditorFilters.ConvertCharactersToEntities);

        RssList = _entities.Rss.Where(p => p.Language == EnrollAdminContext.Current.DataLanguage.LanguageId).ToList();
        FormList =
            _entities.Forms.Where(p => p.LanguageId == EnrollAdminContext.Current.DataLanguage.LanguageId).ToList();
        LList = _entities.Lists.Where(p => p.LanguageId == EnrollAdminContext.Current.DataLanguage.LanguageId).ToList();
        VCategoriesList =
            _entities.VideoCategories.Where(p => p.languageId == EnrollAdminContext.Current.DataLanguage.LanguageId).
                ToList();
        PCategoriesList =
            _entities.Def_photoAlbumCategory.Where(
                p => p.languageId == EnrollAdminContext.Current.DataLanguage.LanguageId).ToList();
        PAlbumsList =
            _entities.Def_photoAlbum.Where(p => p.languageId == EnrollAdminContext.Current.DataLanguage.LanguageId).
                ToList();
    }
}