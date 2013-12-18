<%@ Control Language="C#" AutoEventWireup="true" Inherits="eNroll.Survey"
            ViewStateMode="Enabled" Codebehind="Survey.ascx.cs" %>
<style type="text/css">
    .style1 { height: 10px; }
</style>
<div class="anketTepe" style="font-weight: bold; ">
    <asp:Label ID="anketBaslik" runat="server" />
</div>
<div class="anketGovde">
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="Panel1" runat="server">
                                <p style="margin-top:0;"><asp:Label ID="lblSoru" runat="server" Font-Bold="true">
                                </asp:Label></p>
                                <asp:RadioButtonList ID="RadioButtonList1" runat="server" DataSourceID="EntityDataSource1"
                                                     DataTextField="surveyOption" DataValueField="surveyOptionId" AutoPostBack="True"
                                                     OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
                                </asp:RadioButtonList>
                                <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=Entities"
                                                      DefaultContainerName="Entities" EntitySetName="Survey_Option">
                                </asp:EntityDataSource>
                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                <asp:HiddenField ID="HiddenField2" runat="server" />
                                <asp:HiddenField ID="hdnToplam" runat="server" />
                            </asp:Panel>
                            <asp:Panel ID="Panel4" runat="server" Visible="False">
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="surveyOptionId"
                                              DataSourceID="EntityDataSource1" GridLines="None" OnDataBound="GridView1_DataBound"
                                              ShowHeader="False" Width="100%">
                                    <Columns>
                                        <asp:TemplateField SortExpression="surveyOption">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox2" runat="server" Text='<%#                                        Bind("surveyOption") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%#Bind
                                                                                                   ("surveyOption") %>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="chooseCount">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%#Bind("chooseCount") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblOran" runat="server" CssClass="style1" Font-Bold="True" Text='<%#Bind("chooseCount") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
</div>
<div class="anketAlt">
</div>