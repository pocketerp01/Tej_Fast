if ("undefined" == typeof bobj && (bobj = {}), void 0 === bobj.crv) { if (bobj.crv = {}, bobj.crv.ActxPrintControl_CLSID = "B7DA1CA9-1EF8-4831-868A-A767093EA685", bobj.crv.ActxPrintControl_Version = "13,0,12,1494", bobj.crv.config = { isDebug: !1, scriptUri: null, skin: "skin_standard", needFallback: !0, lang: "en", useCompressedScripts: !0, useAsync: !0, indicatorOnly: !1, resources: { HTMLPromptingSDK: { isLoaded: !1, path: "../../promptengine-compressed.js" }, ParameterControllerAndDeps: { isLoaded: !1, path: "../../parameterUIController-compressed.js" } }, logging: { enabled: !1, id: 0 } }, bobj.crv.logger = { info: function () { } }, "object" == typeof crv_config) for (var i in crv_config) bobj.crv.config[i] = crv_config[i]; if (!bobj.crv.config.scriptUri) { for (var scripts = document.getElementsByTagName("script"), reCrvJs = /(.*)crv\.js$/i, i = 0; i < scripts.length; i++) { var src = scripts[i].getAttribute("src"); if (src) { var matches = src.match(reCrvJs); if (matches && matches.length) { bobj.crv.config.scriptUri = matches[1]; break } } } if (!bobj.crv.config.scriptUri && (bobj.crv.config.scriptUri = "", bobj.crv.config.isDebug)) throw "bobj.crv.config.scriptUri is undefined" } bobj.parseUri = function (r) { var i = r.match(new RegExp("^(([^:/?#]+):)?(//([^/?#]*))?([^?#]*)(\\?([^#]*))?(#(.*))?$")); return { scheme: i[2], authority: i[4], path: i[5], query: i[7], fragment: i[9] } }, bobj.crvUri = function (r) { return bobj.crv.config.scriptUri + r }, bobj.isParentWindowTestRunner = function () { try { for (var r = top || window; r.top && r.top != r;) r = r.top; return void 0 !== r.jsUnitTestSuite } catch (r) { } return !1 }, bobj.skinUri = function (r) { return bobj.crvUri("../dhtmllib/images/") + bobj.crv.config.skin + "/" + r }, bobj.crv.createWidget = function (json) { if (!bobj.isObject(json)) return null; var constructor = json.cons; if (bobj.isString(constructor)) try { constructor = eval(constructor) } catch (r) { if (bobj.crv.config.isDebug) throw "bobj.crv.createWidget: invalid constructor" } if (!bobj.isFunction(constructor)) { if (bobj.crv.config.isDebug) throw "bobj.crv.createWidget: invalid constructor"; return null } var widget = constructor(json.args); if (bobj.isArray(json.children)) if (widget.delayedBatchAdd) widget.delayedBatchAdd(json.children); else if (widget.addChild) for (var i = 0, len = json.children.length; i < len; ++i) { var child = bobj.crv.createWidget(json.children[i]); widget.addChild(child) } return widget }, bobj.crv.writeWidget = function (r, i) { var e = bobj.crv.createWidget(r); if (e) if (i) { var o = e.getHTML(), t = bobj.html.extractHtml(o), n = t.links; i.innerHTML = o; for (var c = 0, s = n.length; c < s; ++c) bobj.includeLink(n[c]); for (var a = t.scripts, b = 0, j = a.length; b < j; ++b) { var l = a[b]; if (l && l.text) try { bobj.evalInWindow(l.text) } catch (r) { } } for (var g = "", d = 0, v = t.styles.length; d < v; d++) g += t.styles[d].text + "\n"; g.length > 0 && bobj.addStyleSheet(g) } else e.write(); return e }, bobj.crv._initWidget = function (r, i) { try { _widgets[r].init(), i && MochiKit.DOM.removeElement(i) } catch (i) { if (bobj.crv.config.isDebug) { var e = "bobj.crv._initWidget: Couldn't initialize widget: "; throw e += "widx=" + r, e += ", type=", _widgets[r] ? bobj.isString(_widgets[r].widgetType) ? e += _widgets[r].widgetType : e += "unknown" : e += "null", e += " because: " + i } } }, bobj.crv.getInitHTML = function (r) { var i = "bobj_crv_getInitHTML_" + r; return '<script id="' + i + '" language="javascript">bobj.crv._initWidget(' + r + ',"' + i + '");<\/script>' }, bobj.crv._preloadProcessingIndicatorImages = function () { for (var r = bobj.crvUri("../dhtmllib/images/" + bobj.crv.config.skin + "/"), i = [], e = ["wait01.gif", "dialogtitle.gif", "dialogelements.gif", "../transp.gif"], o = 0, t = e.length; o < t; o++) i[o] = new Image, i[o].src = r + e[o] }, bobj.crv._loadJavaScript = function (r) { r && document.write('<script language="javascript" src="' + bobj.crvUri(r) + '"><\/script>') }, bobj.crv._includeAll = function () { if (bobj.crv._includeLocaleStrings(), bobj.crv.config.useCompressedScripts) if (bobj.crv.config.indicatorOnly) bobj.crv._loadJavaScript("../../processindicator.js"); else { if (bobj.crv._loadJavaScript("../../allInOne.js"), !bobj.crv.config.useAsync) for (var r in bobj.crv.config.resources) { var i = bobj.crv.config.resources[r]; i.isLoaded = !0, bobj.crv._loadJavaScript(i.path) } bobj.crv.config.logging.enabled && bobj.crv._loadJavaScript("../log4javascript/log4javascript.js") } else { var e = []; e = bobj.crv.config.indicatorOnly ? ["../MochiKit/Base.js", "../dhtmllib/dom.js", "initDhtmlLib.js", "../dhtmllib/dialog.js", "../dhtmllib/waitdialog.js", "common.js", "Dialogs.js"] : ["../MochiKit/Base.js", "../MochiKit/Async.js", "../MochiKit/DOM.js", "../MochiKit/Style.js", "../MochiKit/Signal.js", "../MochiKit/New.js", "../MochiKit/Color.js", "../MochiKit/Iter.js", "../MochiKit/Visual.js", "../log4javascript/log4javascript_uncompressed.js", "../external/date.js", "../dhtmllib/dom.js", "initDhtmlLib.js", "../dhtmllib/palette.js", "../dhtmllib/menu.js", "../dhtmllib/psheet.js", "../dhtmllib/treeview.js", "../dhtmllib/dialog.js", "../dhtmllib/waitdialog.js", "../../prompting/js/promptengine_prompts2.js", "../../prompting/js/promptengine_calendar2.js", "../swfobject/swfobject.js", "common.js", "encoding.js", "html.js", "ImageSprites.js", "Toolbar.js", "Statusbar.js", "PanelNavigator.js", "PanelNavigatorItem.js", "PanelHeader.js", "LeftPanel.js", "GroupTreeNode.js", "GroupTree.js", "GroupTreeListener.js", "ToolPanel.js", "ReportPage.js", "ReportView.js", "ButtonList.js", "ReportAlbum.js", "Separator.js", "Viewer.js", "ViewerListener.js", "StateManager.js", "IOAdapters.js", "ArgumentNormalizer.js", "event.js", "PromptPage.js", "Dialogs.js", "StackedTab.js", "StackedPanel.js", "Parameter.js", "ParameterController.js", "Colors.js", "TextField.js", "TextCombo.js", "RangeField.js", "ParameterValueRow.js", "ParameterInfoRow.js", "OptionalParameterValueRow.js", "ParameterUI.js", "OptionalParameterUI.js", "ParameterDialog.js", "ParameterPanelToolbar.js", "ParameterPanel.js", "bobjcallback.js", "Calendar.js", "WarningPopup.js", "../FlexParameterBridge.js", "ViewerFlexParameterAdapter.js", "SignalDisposer.js"], bobj.isParentWindowTestRunner() && e.push("../jsunit/tests/crViewerTestSuite.js"); for (var o = 0, t = e.length; o < t; o++) document.write('<script language="javascript" src="' + bobj.crvUri(e[o]) + '"><\/script>'); for (var o in bobj.crv.config.resources) bobj.crv.config.resources[o].isLoaded = !0 } }, bobj.crv.getLangCode = function () { var r = "_"; bobj.crv.config.lang.indexOf("-") > 0 && (r = "-"); var i = bobj.crv.config.lang.split(r); if ("zh" == i[0].toLowerCase()) if (i.length > 1) { var e = i[1].toUpperCase(); i[1] = "TW" == e || "HK" == e || "MO" == e || "MY" == e ? "TW" : "CN" } else i[1] = "CN"; return i = i.length > 1 && (!bobj.crv.config.needFallback || "zh" == i[0].toLowerCase()) ? i[0] + "_" + i[1] : i[0] }, bobj.crv._includeLocaleStrings = function () { var r = bobj.crv.getLangCode(); bobj.crv.config.needFallback && (bobj.crv._loadJavaScript("../../allStrings_en.js"), "en" == r) || bobj.crv._loadJavaScript("../../allStrings_" + r + ".js") }, "undefined" == typeof MochiKit && (MochiKit = {}), void 0 === MochiKit.__export__ && (MochiKit.__export__ = !1), bobj.crv.config.useAsync || bobj.crv._preloadProcessingIndicatorImages(), bobj.crv._includeAll(), bobj.crv.initLog = function (r, i) { if (bobj.crv.logger = log4javascript.getLogger(), log4javascript.setEnabled(bobj.crv.config.logging.enabled), bobj.crv.config.logging.enabled) { bobj.crv.logger.setLevel(r); var e = i + "?ServletTask=Log", o = new log4javascript.AjaxAppender(e); bobj.crv.logger.addAppender(o); var t = bobj.crv.logger.log; bobj.crv.logger.log = function (r, i, e) { t(r, bobj.crv.config.logging.id + " " + i, e) }, bobj.crv.logger.info("Logging Initialized") } }, bobj.crv.invokeActionAdapter = function (r) { var i = "invokeAction", e = window[i]; !e && window.parent && (e = window.parent[i]), e && e(r.url, r.actionId, r.objectId, r.containerId, r.actionType, null) } } "undefined" != typeof Sys && void 0 !== Sys.Application && void 0 !== Sys.Application.notifyScriptLoaded && Sys.Application.notifyScriptLoaded();