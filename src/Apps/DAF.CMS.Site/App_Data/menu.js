[
{
    Name: "admin", Caption: "系统管理", MenuItems: [
        {
            Name: "cmsManage", Caption: "网站管理", LinkUrl: "#", ProtectedUri: ""
            , Children: [
                { Name: "cateManage", Caption: "栏目设置", LinkUrl: "/Category/Cate?client=DAF.CMS.Site&title=栏目管理", ProtectedUri: "" }
                , {
                    Name: "newContent", Caption: "新增内容", LinkUrl: "#", ProtectedUri: ""
                    , Children: [
                        { Name: "newHtml", Caption: "新增Html内容", LinkUrl: "/Content/Edit/Html", ProtectedUri: "" }
                        , { Name: "newImage", Caption: "新增图片", LinkUrl: "/Content/Edit/Image", ProtectedUri: "" }
                        , { Name: "newFile", Caption: "新增下载文件", LinkUrl: "/Content/Eidt/File", ProtectedUri: "" }
                        , { Name: "newLink", Caption: "新增链接", LinkUrl: "/Content/Edit/Link", ProtectedUri: "" }
                        , { Name: "newText", Caption: "新增文本", LinkUrl: "/Content/Edit/Text", ProtectedUri: "" }
                        , { Name: "newAudio", Caption: "新增音频", LinkUrl: "/Content/Edit/Audio", ProtectedUri: "" }
                        , { Name: "newVideo", Caption: "新增视频", LinkUrl: "/Content/Edit/Video", ProtectedUri: "" }
                        , { Name: "newOrg", Caption: "新增组织机构", LinkUrl: "/Content/Edit/Org", ProtectedUri: "" }
                        , { Name: "newPerson", Caption: "新增人物", LinkUrl: "/Content/Edit/Person", ProtectedUri: "" }
                        , { Name: "newContact", Caption: "新增联系方式", LinkUrl: "/Content/Edit/Contact", ProtectedUri: "" }
                    ]
                }
                , { Name: "articalManage", Caption: "内容管理", LinkUrl: "/Content", ProtectedUri: "" }
                , { Name: "settingsManage", Caption: "站点设置", LinkUrl: "/AppSettings/Client?client=DAF.CMS.Site&title=站点设置", ProtectedUri: "" }
                , { Name: "adManage", Caption: "广告管理", LinkUrl: "/Ad", ProtectedUri: "" }
            ]
        }
        , {
            Name: "otherManage", Caption: "常用管理", LinkUrl: "#", ProtectedUri: "",
            Children: [
                { Name: "categoryManage", Caption: "分类管理", LinkUrl: "/Category", ProtectedUri: "" }
                , { Name: "basicDataManage", Caption: "基础数据", LinkUrl: "/BasicData", ProtectedUri: "" }
                , { Name: "menuManage", Caption: "菜单管理", LinkUrl: "/Menu", ProtectedUri: "" }
                , { Name: "appSettingsManage", Caption: "参数设置", LinkUrl: "/AppSettings", ProtectedUri: "" }
                , { Name: "userGroupManage", Caption: "用户分组", LinkUrl: "/UserGroup", ProtectedUri: "" }
            ]
        }
    ]
}
]
