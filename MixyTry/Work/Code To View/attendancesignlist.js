/// <reference path="\Assets\common\ycf_admin.js"/> 【不要删除该注释】

(function () {

    $(function () {
        $("body").append($('<div id="signtooltip"  class="highlight  table-td-green" style="display:none">  </div>'));

        div_searchCondition.init();

        div_tableList.loadTableList();

        EventHandler.bindEvent();
    });

    var div_searchCondition = (function () {
        function div_searchCondition() { }

        function initBDSelect() {
            var hasCurrentUser = false;
            $.each(renderData.BDSelect, function (i, val) {
                $('#sl_BD').append('<option text="' + val.Text + '" value="' + val.Value + '">' + val.DisplayText + '</option>');
                if (val.Value == renderData.CurrentUserId) {
                    hasCurrentUser = true;
                }
            });
            $("#sl_BD").select2({
                placeholder: "Select a State",
                allowClear: true
            });
            $("#sl_BD").select2('val', hasCurrentUser ? renderData.CurrentUserId : renderData.BDSelect[0].Value);
        }

        function initSignStatusSelect() {
            $.each(renderData.SignStatusSelect, function (i, val) {
                $('#sl_SignStatus').append('<option text="' + val.Text + '" value="' + val.Value + '">' + val.DisplayText + '</option>');
            });
            $("#sl_SignStatus").select2({
                placeholder: "Select a State",
                allowClear: true
            });
            $("#sl_SignStatus").select2('val', renderData.SignStatusSelect[0].Value);
        }

        function initOtherWidget() {
            var now = new Date();
            window.__minDate2 = Utility.addDays(new Date(), -93);
            window.__maxDate2 = now;

            $("#txtStartDate").val(Utility.formatDate(now));
            $("#txtEndDate").val(Utility.formatDate(now));
            $('input[type="radio"][name="ExcludeHoliday"][value="true"]').prop("checked", "true");
        }

        div_searchCondition.init = function () {
            initBDSelect();
            initSignStatusSelect();
            initOtherWidget();
        }

        return div_searchCondition;
    }());

    var div_tableList = (function () {

        function div_tableList() { }

        var toolbar = [{
            id: 'btn_Export',
            text: ' 导出',
            iconCls: 'icon-pencil',
            handler: function () {
                ExportExcel();
            }
        }];

        var i = 1;

        var columns = [[{
            //2
            field: 'DateDisplay',
            title: '日期',
            align: 'center',
            width: 80,
            sortable: true,
            order: 'asc'
        }, {
            //3
            field: 'LoginName',
            title: 'BD',
            align: 'center',
            width: 80,
            sortable: true,
            order: 'asc'
        }, {
            //4
            field: 'DispalyDic' + i++,
            title: '00：00-02：00',
            align: 'center',
            width: 80,
            formatter: function (value, rowData, rowIndex) {
                var mark = "0000";
                return formaterTimeDisplay(rowData, mark);

                //return '<span class="workInCompany "></span>';
            }
        }, {
            //5
            field: 'DispalyDic2' + i++,
            title: '02：00-04：00',
            align: 'center',
            width: 80,
            formatter: function (value, rowData, rowIndex) {
                var mark = "0200";
                return formaterTimeDisplay(rowData, mark);

            }
        }, {
            //6
            field: 'DispalyDic3' + i++,
            title: '04：00-06：00',
            align: 'center',
            width: 80,
            formatter: function (value, rowData, rowIndex) {
                var mark = "0400";
                return formaterTimeDisplay(rowData, mark);
            }
        }, {
            //7
            field: 'DispalyDic4' + i++,
            title: '06：00-08：00',
            align: 'center',
            width: 80,
            formatter: function (value, rowData, rowIndex) {
                var mark = "0600";
                return formaterTimeDisplay(rowData, mark);
            }
        }, {
            //8
            field: 'DispalyDic' + i++,
            title: '08：00-10：00',
            align: 'center',
            width: 80,
            formatter: function (value, rowData, rowIndex) {
                var mark = "0800";
                return formaterTimeDisplay(rowData, mark);
            }
        }, {
            //9
            field: 'DispalyDic' + i++,
            title: '10：00-12：00',
            align: 'center',
            width: 80,
            formatter: function (value, rowData, rowIndex) {
                var mark = "1000";
                return formaterTimeDisplay(rowData, mark);
            }
        }, {
            //10
            field: 'DispalyDic' + i++,
            title: '12：00-14：00',
            align: 'center',
            width: 80,
            formatter: function (value, rowData, rowIndex) {
                var mark = "1200";
                return formaterTimeDisplay(rowData, mark);
            }
        }, {
            //11
            field: 'DispalyDic' + i++,
            title: '14：00-16：00',
            align: 'center',
            width: 80,
            formatter: function (value, rowData, rowIndex) {
                var mark = "1400";
                return formaterTimeDisplay(rowData, mark);
            }
        }, {
            //12
            field: 'DispalyDic' + i++,
            title: '16：00-18：00',
            align: 'center',
            width: 80,
            formatter: function (value, rowData, rowIndex) {
                var mark = "1600";
                return formaterTimeDisplay(rowData, mark);
            }
        }, {
            //13
            field: 'DispalyDic' + i++,
            title: '18：00-20：00',
            align: 'center',
            width: 80,
            formatter: function (value, rowData, rowIndex) {
                var mark = "1800";
                return formaterTimeDisplay(rowData, mark);
            }
        }, {
            //14
            field: 'DispalyDic' + i++,
            title: '20：00-22：00',
            align: 'center',
            width: 80,
            formatter: function (value, rowData, rowIndex) {
                var mark = "2000";
                return formaterTimeDisplay(rowData, mark);
            }
        }, {
            //15
            field: 'DispalyDic' + i++,
            title: '22：00-24：00',
            align: 'center',
            width: 80,
            formatter: function (value, rowData, rowIndex) {
                var mark = "2200";
                return formaterTimeDisplay(rowData, mark);
            }
        }]];

        var formaterTimeDisplay = function (value, displayMark) {
            if (!value.Dic[displayMark]) {
                return '<span   onmouseover="SignTooltip.ShowShouldMark(this)"  cus-Id="' + value.Id + '", cus-displayMark="' + displayMark + '" >' + SignTooltip.Getblank() + '</span>'
            } else if (value.Dic[displayMark].SignType == 0) {
                ///// <summary>
                ///// 未签到
                ///// </summary>
                //[Description("未签到备注")]
                //NotSignMark = 0,

                return '<span onmousemove="SignTooltip.ShowNotSignMark(this)" onmouseout="SignTooltip.HideTooltip(this)" cus-Id="' + value.Id + '", cus-displayMark="' + displayMark + '" >备注 </span>'
            } else if (value.Dic[displayMark].SignType == 1) {
                ///// <summary>
                ///// 公司签到
                ///// </summary>
                //[Description("公司签到")]
                //WorkInCompany = 1,
                return "公司签到";

            } else if (value.Dic[displayMark].SignType == 2) {
                ///// <summary>
                ///// 外出拜访
                ///// </summary>
                //[Description("外出拜访")]
                //WorkOnTheField = 2,
                return '<span onmousemove="SignTooltip.ShowWorkOnTheField(this)" onmouseout="SignTooltip.HideTooltip(this)"  cus-Id="' + value.Id + '", cus-displayMark="' + displayMark + '" >外出拜访</span>'
            }
        }

        var ExportExcel = function () {

            var url = "/Srm/AttendanceSign/ExportExcel";
            url += "?&BDLoginId=" + $("#sl_BD").val();
            url += "&StartDate=" + $("#txtStartDate").val();
            url += "&EndDate=" + $("#txtEndDate").val();
            url += "&AttendanceSignStatus=" + $("#sl_SignStatus").val();
            url += "&ExcludeHoliday=" + $('input[type="radio"][name="ExcludeHoliday"]:checked').val();
            url += "&page=1&rows=10000000"
            window.open(url);
        }

        var getAndCheckParameters = function () {
            var para = {
                BDLoginId: $("#sl_BD").val(),
                StartDate: $("#txtStartDate").val(),
                EndDate: $("#txtEndDate").val(),
                AttendanceSignStatus: $("#sl_SignStatus").val(),
                ExcludeHoliday: $('input[type="radio"][name="ExcludeHoliday"]:checked').val()
            };
            return para;
        }

        div_tableList.loadTableList = function () {
            ycf_admin.loadGrid($('#tb_attendanceSignList'), {
                title: null,
                url: '/Srm/AttendanceSign/GetAttendanceSignList',
                loadMsg: '努力加载中...',
                queryParams: getAndCheckParameters(),
                sortName: 'DateDisplay',
                fit: true,
                fitColumns: true,
                striped: true,
                height: 400,
                pageSize: 20,
                //idField: 'ChannelLinkId',
                columns: columns,
                pagination: true,
                rownumbers: true,
                singleSelect: true,
                selectOnCheck: true,
                checkOnSelect: true,
                toolbar: toolbar
            }, null, null, function (data) {
                listData = data.rows;
            });
        }

        return div_tableList;
    }());

    window.__minDate2 = null;
    window.__maxDate2 = null;

    var Utility = (function () {
        function Utility() { }

        Utility.addDays = function (someDate, days) {
            someDate.setDate(someDate.getDate() + days);
            return someDate;
        };

        Utility.formatDate = function formatDate(date) {
            month = '' + (date.getMonth() + 1),
            day = '' + date.getDate(),
            year = date.getFullYear();

            if (month.length < 2) month = '0' + month;
            if (day.length < 2) day = '0' + day;

            return [year, month, day].join('-');
        };

        return Utility;
    }());

    var listData;

    window.__minDate1 = "2008-06-07 00:00"
    window.__maxDate1 = "2008-06-07 00:00";

    var EventHandler = (function () {
        function EventHandler() { }

        EventHandler.bindEvent = function () {

            $("#btn_search").on("click", function () {
                div_tableList.loadTableList();
            });

            $("#mark-yes").on("click", function () {

                debugger;
                Modal.confirm({ msg: '是否确认备注？（备注后无法修改）' }).on(function (e) {
                    if (e) {
                        App.blockUI($("#div_modelmark"));
                        $.ajax({
                            type: "POST",
                            url: "/Srm/AttendanceSign/MarkNotSign",
                            dataType: "json",
                            data: {
                                StartDate: $("#MarkStartDate").val(),
                                EndDate: $("#MarkEndDate").val(),
                                Remark: $("#not-sign-mark").val(),
                                LoginId: SignTooltip.GetCellData().model.LoginId
                            },
                            success: function (jsonData) {
                                if (jsonData.ResultType == 0) {
                                    console.log("sussecc");
                                } else {
                                    Modal.alert({ msg: jsonData.Message });
                                }
                            },
                            error: function (xmlhttprequest, textstatus, errorthrown) {
                                Modal.alert({ msg: "异常,请联系管理员" });
                            },
                            complete: function () {
                                App.unblockUI($("#div_modelmark"));
                                $("#mark-no").trigger("click");
                                $('#tb_attendanceSignList').datagrid('reload')
                            }
                        });
                    }
                });
            });
        };

        EventHandler.tooltip = (function () {
            function TTTT() { };

            var blank2 = "<br>";
            var blank1 = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            var blank = blank1 + blank2 + blank1;

            var cellData = null;

            function showTooltip(css) {

                $("#signtooltip").show().css({
                    "left": window.event.screenX + 10,
                    "top": window.event.screenY - 75,
                    "z-index": "9000",
                    "display": "block",
                    "position": "fixed",
                    'background': 'white',
                    'max-width': "400px",
                    "border": "solid 1px black",
                });

                if (($("#signtooltip").width() + window.event.screenX) > window.document.body.offsetWidth) {
                    $("#signtooltip").css({
                        left: window.event.screenX - $("#signtooltip").width()
                    })
                }
            }

            function RetrieveModel(element) {
                var id = $(element).attr("cus-Id");
                var displayMark = $(element).attr("cus-displayMark");
                var model
                $.each(listData, function (index, value) {
                    if (value.Id == id) {
                        model = value;
                    }
                })
                return {
                    Id: id,
                    displayMark: displayMark,
                    model: model
                }
            }

            TTTT.Getblank = function () {
                return blank;
            }

            TTTT.GetCellData = function () {
                return cellData;
            };

            TTTT.ShowShouldMark = function (element) {
                $(element).parent().parent().one("mouseleave", function () {
                    $(element).html(blank);
                    console.log("leave");
                });
                if ($(element).html() == blank) {
                    cellData = RetrieveModel(element);
                    $(element).html('<a onclick="SignTooltip.showMarkNotSignModel(this)">点击填写备注</a>');
                }
            }

            TTTT.showMarkNotSignModel = function (element) {
                $("#not-sign-mark").val("");
                var data = cellData;
                var time = data.displayMark.slice(0, 2);
                var startTime = data.model.DateTimeStr + " " + time + ":00";
                var timeend = parseInt(time) + 2
                var endTime = data.model.DateTimeStr + " " + timeend.toString() + ":00";

                if (timeend == 24) {
                    timeend = timeend - 1;
                    endTime = data.model.DateTimeStr + " " + timeend.toString() + ":59"
                }

                $("#MarkStartDate").val(startTime);
                $("#MarkEndDate").val(endTime);
                window.__minDate1 = data.model.DateTimeStr + " 00:00";
                window.__maxDate1 = data.model.DateTimeStr + " 23:59";

                $("#div_modelmark").modal({
                    width: 800
                });
            }

            TTTT.ShowWorkOnTheField = function (element) {
                if ($("#signtooltip").children().length == 0) {
                    var data = RetrieveModel(element);
                    var displayMark = data.displayMark;
                    var model = data.model;

                    var htmls = [];
                    htmls.push('<div>签到酒景：' + model.Dic[displayMark].ProductName + '</div>');
                    htmls.push('<div>酒景地址：' + model.Dic[displayMark].ProductAddress + '</div>');
                    htmls.push('<div>签到地址：' + model.Dic[displayMark].SignLocation + '</div>');
                    htmls.push('<div>备注：' + model.Dic[displayMark].Remark + '</div>');
                    $("#signtooltip").children().remove();
                    var str = htmls.join("");
                    $("#signtooltip").html(str);
                }

                showTooltip();
            }

            TTTT.showTooltip = showTooltip;

            TTTT.ShowNotSignMark = function (element) {
                var data = RetrieveModel(element);
                var model = data.model;
                var displayMark = data.displayMark;
                debugger;
                var htmls = [];
                htmls.push('<div>备注：' + model.Dic[displayMark].Remark + '</div>');

                $("#signtooltip").children().remove();
                var str = htmls.join("");
                $("#signtooltip").html(str);
                showTooltip();
            }

            TTTT.HideTooltip = function (element) {
                $("#signtooltip").children().remove();
                $("#signtooltip").hide();
                console.log("out");
            }

            return TTTT;
        }());

        window.SignTooltip = EventHandler.tooltip;

        return EventHandler;
    }());

}())

