<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Finance_Beta.index" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Finance</title>
    <!--<meta http-equiv="refresh" content="60" />-->
    <script src="Scripts/jquery-1.12.4.min.js"></script>
    <script src="Scripts/jquery-ui-1.12.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>

    <link href="Content/themes/base/jquery-ui.min.css" rel="stylesheet" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="css/index.css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container">
            <div class="row">
                <div class="col-sm-3 col-sm-offset-8">
                    <asp:Panel runat="server" DefaultButton="btnSearch">
                        <div class="input-group">
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control fix-top"></asp:TextBox>
                            <span class="input-group-btn">
                                <asp:LinkButton ID="btnSearch" CssClass="btn btn-default glyphicon glyphicon-search" type="submit" runat="server" OnClick="btnSearch_Click"></asp:LinkButton>
                            </span>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>

        <script type="text/javascript">
        $(document).ready(function () {
            $('#txtSearch').autocomplete({
                source: 'companySearch.ashx'
            });
        });
    </script>

        <div class="container">
            <div class="row">
                <div class="col-sm-12"><h2><asp:Label ID="name" runat="server"></asp:Label></h2></div>
                <div class="col-sm-6">
                    <div class="col-sm-4">
                        <b><asp:Label ID="stockPrice" runat="server"></asp:Label></b>
                        <i><asp:Label ID="change_PercentChange" runat="server"></asp:Label></i>
                    </div>
                    <div class="col-sm-8">
                        <div class="col-sm-6">
                            <label>Mkt cap:</label> <asp:Label ID="marketCapitalization" runat="server"></asp:Label>
                        </div>
                        <div class="col-sm-6">
                            <label>Highest:</label> <asp:Label ID="daysHigh" runat="server"></asp:Label>
                        </div>
                        <div class="col-sm-6">
                            <label>Lowest:</label> <asp:Label ID="daysLow" runat="server"></asp:Label>
                        </div>
                        <div class="col-sm-6">
                            <label>Currency:</label> <asp:Label ID="currency" runat="server"></asp:Label>
                        </div>
                    </div>   
                </div>
                <div class="col-sm-12"><asp:Label ID="poweredby" runat="server"></asp:Label></div>
            </div>
        </div>  

        <div class="container panel panel-default container">
            <div class="row">

                <asp:UpdatePanel ID="chartUpdate" runat="server">
                    <ContentTemplate>
                        <div class="col-sm-7">
                            <div class="col-sm-7">
                                <div class="col-sm-1">Month: </div>
                                <div class="col-sm-1 col-sm-offset-2"><asp:linkbutton runat="server" CommandArgument="1" OnClick="historicTimeLine_Click">1</asp:linkbutton></div>
                                <div class="col-sm-1"><asp:linkbutton  runat="server" CommandArgument="3" OnClick="historicTimeLine_Click">3</asp:linkbutton></div>
                                <div class="col-sm-1"><asp:linkbutton  runat="server" CommandArgument="6" OnClick="historicTimeLine_Click">6</asp:linkbutton></div>
                                <div class="col-sm-1"><asp:linkbutton  runat="server" CommandArgument="12" OnClick="historicTimeLine_Click">12</asp:linkbutton></div>
                            </div>
                            <asp:Chart ID="Chart1" runat="server" Height="300" Width="650">
                                <Series>
                                    <asp:Series Name="Series1" ChartType="FastLine" ToolTip="Date: #VALX\nValue: #VALY">
                                        <Points>
                                        </Points>
                                    </asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1">
                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>

                <div class="panel panel-default col-sm-5">
                    <div class="panel-heading">
                            <i class="fa fa-bar-chart-o fa-fw"></i> News
                        </div>
                    <div class="panel-body">
                <div class="col-sm-12 news">
                    <asp:Repeater ID="newsList" Runat="server" EnableViewState="False">
                        <ItemTemplate>
                            <div class="col-sm-10 newsHeader">
                                <asp:HyperLink ID="urlId" runat="server" Text='<%# Eval("title") %>' NavigateUrl='<%# Eval("url") %>'></asp:HyperLink>
                                <br />
                                <asp:Label runat="server" Text='<%# Eval("publishDate") %>'></asp:Label>
                            </div>
                            <div class="col-sm-2"><a href="#" title="Keywords" data-html="true" data-container="body" data-toggle="popover" data-placement="left" data-content='<%#  watsonKeywords(Eval("url").ToString()) %>'><%# watson(Eval("url").ToString()) %></a></div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                </div>
                </div>

            </div>
        </div>

        <div class="panel panel-default container">
            <div class="panel-heading">
                <i class="fa fa-bar-chart-o fa-fw"></i> Income Statement
            </div>
        <div class="panel-body">
        <div class="container">
            <div class="row">
                <div class="col-sm-4">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" >
                         <Columns>
                            <asp:BoundField DataField="year" HeaderText="Year" 
                                InsertVisible="False" ReadOnly="True" />
                            <asp:BoundField DataField="totalRevenue" HeaderText="Total revenue"  />
                            <asp:BoundField DataField="CostOfRevenue" HeaderText="Cost of revenue"  />
                            <asp:BoundField DataField="GrossProfit" HeaderText="Gross profit"  />
                             <asp:BoundField DataField="NetIncome" HeaderText="Net income"  />
                        </Columns>
                     </asp:GridView>
                </div>
                <div class="col-sm-6 col-sm-push-1">
                    <asp:Chart ID="Chart2" runat="server" Height="300" Width="600">
                    <Series>
                        <asp:Series Name="Series1" ToolTip="#VALY">
                            <Points>
                            </Points>
                        </asp:Series>
                    </Series>
                    <Series>
                        <asp:Series Name="Series2" ToolTip="#VALY">
                            <Points>
                            </Points>
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1">
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            <label>(In Millions)</label>
            </div>
        </div>
    </div>
    </div>
    </div>
    </form>

<script>
$(document).ready(function(){
    $('[data-toggle="popover"]').popover();   
});
</script>

</body>
</html>
