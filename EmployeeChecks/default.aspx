<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="EmployeeChecks._default" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>VoxproData - Employee Checks</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" rel="stylesheet"/>
    <script type="text/javascript" src="Scripts/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <link href="Scripts/styles.css" rel="stylesheet" />
    <script type="text/javascript">

        var queryVisible = false;

        function ShowProgress() {

            console.log("ShowProgress Function");
            $('#no_interaction').show();
            $('#loading_done_button').hide();

            var up = Math.max($(window).height() / 2 - $("#loading_modal").outerHeight() / 2, 0);
            var side = Math.max($(window).width() / 2 - $("#loading_modal").outerWidth() / 2, 0);

            var styles = {
                top: up,
                left: side
            };

            $("#loading_modal").css(styles);
            $("#loading_modal").show();
        }

        $(document).ready(function () {
            $("#loading_modal").hide();
            $('#no_interaction').hide();

            $('#genesys_ok').hide();
            $('#genesys_modify').hide();
            $('#genesys_wait').hide();

            $('#impact_ok').hide();
            $('#impact_modify').hide();
            $('#impact_wait').hide();

            $('#injixo_ok').hide();
            $('#injixo_modify').hide();
            $('#injixo_wait').hide();
        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-default">
            <div class="container-fluid">
                <div class="navbar-header">
                    <a class="navbar-brand abs" href="#"><img src="img/voxpro_data_logo.png" /></a>
                </div>
                <!-- <ul class="nav navbar-nav navbar-right">
                    <li>
                        <asp:Button ID="CustomSqlQueryBt" CssClass="btn btn-default" runat="server" Text="Show Custom Query" />
                    </li>
                </ul> -->
                <asp:Label ID="sessioname" runat="server" Text="" CssClass="nav navbar-nav navbar-right"></asp:Label>
            </div>
        </nav>
        <div class="container-fluid col-lg-12">
            <p>
                <div>
                    <asp:Button ID="genesys_check" runat="server" Text="Check GenesysWFM" CssClass="btn btn-sm btn-primary" />
                    <asp:Image ID="genesys_ok" runat="server" ImageUrl="~/img/green.svg" Height="30px" Width="30px" />
                    <asp:ImageButton ID="genesys_modify" runat="server" ImageUrl="~/img/modify.svg" Height="30px" Width="30px"/>
                    <asp:ImageButton ID="genesys_wait" runat="server" ImageUrl="~/img/Facebook.svg" />
                </div>
                <asp:GridView ID="genesys_results_grid" runat="server" Width="1000px" CssClass="table table-striped table-bordered table-hover" BorderStyle="None" GridLines="None" ShowHeaderWhenEmpty="True" AlternatingRowStyle-Wrap="False" FooterStyle-Wrap="False" HeaderStyle-Wrap="False" PagerStyle-Wrap="False" SelectedRowStyle-Wrap="False" SortedAscendingCellStyle-Wrap="False" SortedAscendingHeaderStyle-Wrap="False" SortedDescendingHeaderStyle-Wrap="False"></asp:GridView>
            </p>
            <p>
                <div>
                    <asp:Button ID="impact360_check" runat="server" Text="Check Impact360" CssClass="btn btn-sm btn-primary"/>
                    <asp:Image ID="impact_ok" runat="server" ImageUrl="~/img/green.svg" Height="30px" Width="30px" />
                    <asp:ImageButton ID="impact_modify" runat="server" ImageUrl="~/img/modify.svg" Height="30px" Width="30px"/>
                    <asp:Image ID="impact_wait" runat="server" ImageUrl="~/img/Facebook.svg" />
                </div>
                <asp:GridView ID="impact360_results_grid" runat="server"></asp:GridView>
            </p>
            <p>
                <div>
                    <asp:Button ID="injixo_check" runat="server" Text="Check Injixo" CssClass="btn btn-sm btn-primary"/>
                    <asp:Image ID="injixo_ok" runat="server" ImageUrl="~/img/green.svg" Height="30px" Width="30px" />
                    <asp:ImageButton ID="injixo_modify" runat="server" ImageUrl="~/img/modify.svg" Height="30px" Width="30px"/>
                    <asp:Image ID="injixo_wait" runat="server" ImageUrl="~/img/Facebook.svg" />
                </div>
                <asp:GridView ID="injixo_results_grid" runat="server"  Width="1000px" CssClass="table table-striped table-bordered table-hover" BorderStyle="None" GridLines="None" ShowHeaderWhenEmpty="True" AlternatingRowStyle-Wrap="False" FooterStyle-Wrap="False" HeaderStyle-Wrap="False" PagerStyle-Wrap="False" SelectedRowStyle-Wrap="False" SortedAscendingCellStyle-Wrap="False" SortedAscendingHeaderStyle-Wrap="False" SortedDescendingHeaderStyle-Wrap="False">
                    <AlternatingRowStyle BackColor="#CCFFFF" Font-Bold="False" />
                    <HeaderStyle BackColor="#3399FF" Wrap="False" />
                </asp:GridView>
            </p>
            <asp:Label ID="QueryMessages_txt" runat="server" Text="" ForeColor="#990000"></asp:Label>
        </div>
    </form>
    <div id="wait">
        <div id="no_interaction" runat="server"></div>
        <div id="loading_modal" runat="server">
            <div id="query_message" runat="server">Querying. Please wait</div>
            <div id="progress_bar" style="width: 60px; height: 10px; border: 1px solid black;">
                <div style="width: 0px; height: 10px; background-color: green;" id="FillBar" runat="server">&nbsp;</div>
            </div>
            <img id="hourglass" src="img/hourglass.svg" runat="server"/>
            <button id="loading_done_button" class="btn btn-sm btn-success" runat="server" style="display:none;">Done!</button>
        </div>
    </div>
</body>
</html>
