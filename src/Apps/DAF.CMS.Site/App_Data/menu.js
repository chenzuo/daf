[
{
    Name: "admin", Caption: "系统管理", MenuItems: [
        {
            Name: "manager", Caption: "系统管理", LinkUrl: "#", ProtectedUri: "", Children: [
                { Name: "userManage", Caption: "用户管理", LinkUrl: "{cmd=url with:sso_server}/User", ProtectedUri: "SysAdmin" }
                , { Name: "roleManage", Caption: "角色管理", LinkUrl: "{cmd=url with:sso_server}/Role", ProtectedUri: "SysAdmin" }
                , { Name: "permissionManage", Caption: "受保护资源", LinkUrl: "{cmd=url with:sso_server}/Permission", ProtectedUri: "SysAdmin" }
                , { Name: "articalManage", Caption: "网站管理", LinkUrl: "/Admin/WebSite", ProtectedUri: "" }
                , { Name: "userGroupManage", Caption: "用户分组", LinkUrl: "/Admin/UserGroup", ProtectedUri: "" }
                , { Name: "settingsManage", Caption: "站点设置", LinkUrl: "/Admin/AppSettings", ProtectedUri: "" }
                , { Name: "basicDataManage", Caption: "基础数据", LinkUrl: "/Admin/BasicData", ProtectedUri: "" }
        ]}
        , {
            Name: "designer", Caption: "网站设计", LinkUrl: "#", ProtectedUri: "", Children: [
                { Name: "pageTemplateManage", Caption: "页面模板", LinkUrl: "/Admin/PageTemplate", ProtectedUri: "" }
                , { Name: "cateManage", Caption: "栏目设置", LinkUrl: "/Admin/Category", ProtectedUri: "" }
                , { Name: "menuManage", Caption: "菜单管理", LinkUrl: "/Admin/Menu", ProtectedUri: "" }
                , { Name: "pageManage", Caption: "页面管理", LinkUrl: "/Admin/Page", ProtectedUri: "" }
            ]
        }
        , {
            Name: "editor", Caption: "内容编辑", LinkUrl: "#", ProtectedUri: "", Children: [
                { Name: "articalManage", Caption: "内容管理", LinkUrl: "/Admin/Content", ProtectedUri: "" }
            ]
        }
    ]
}
]
