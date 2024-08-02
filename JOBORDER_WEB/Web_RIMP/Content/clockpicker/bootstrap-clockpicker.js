(function (t) { function e(t) { return document.createElementNS(p, t) } function i(t) { return (t < 10 ? "0" : "") + t } function s(t) { var e = ++b + ""; return t ? t + e : e } function o(o, r) { function c(t, e) { var i = u.offset(), s = /^touch/.test(t.type), o = i.left + g, n = i.top + g, c = (s ? t.originalEvent.touches[0] : t).pageX - o, p = (s ? t.originalEvent.touches[0] : t).pageY - n, d = Math.sqrt(c * c + p * p), f = !1; if (!e || !(d < w - C || d > w + C)) { t.preventDefault(); var v = setTimeout(function () { a.addClass("clockpicker-moving") }, 200); l && u.append(x.canvas), x.setHand(c, p, !0), h.off(k).on(k, function (t) { t.preventDefault(); var e = /^touch/.test(t.type), i = (e ? t.originalEvent.touches[0] : t).pageX - o, s = (e ? t.originalEvent.touches[0] : t).pageY - n; (f || i !== c || s !== p) && (f = !0, x.setHand(i, s, !0)) }), h.off(m).on(m, function (t) { h.off(m), t.preventDefault(); var i = /^touch/.test(t.type), s = (i ? t.originalEvent.changedTouches[0] : t).pageX - o, l = (i ? t.originalEvent.changedTouches[0] : t).pageY - n; (e || f) && s === c && l === p && x.setHand(s, l), "hours" === x.currentView ? x.toggleView("minutes", duration / 2) : r.autoclose && (r.ampmSubmit || (x.minutesView.addClass("clockpicker-dial-out"), setTimeout(function () { x.done() }, duration / 2))), u.prepend(N), clearTimeout(v), a.removeClass("clockpicker-moving"), h.off(k) }) } } var p = t(M), u = p.find(".clockpicker-plate"), d = p.find(".clockpicker-hours"), v = p.find(".clockpicker-minutes"), b = "INPUT" === o.prop("tagName"), A = b ? o : o.find("input"), H = "time" === A.prop("type"), T = o.find(".input-group-addon"), P = p.find(".popover-body"), V = P.find(".clockpicker-close-block"), x = this; this.id = s("cp"), this.element = o, this.options = r, this.options.hourstep = this.parseStep(this.options.hourstep, 12), this.options.minutestep = this.parseStep(this.options.minutestep, 60), this.isAppended = !1, this.isShown = !1, this.currentView = "hours", this.isInput = b, this.isHTML5 = H, this.input = A, this.addon = T, this.popover = p, this.plate = u, this.hoursView = d, this.minutesView = v, this.spanHours = p.find(".clockpicker-span-hours"), this.spanMinutes = p.find(".clockpicker-span-minutes"), this.buttonsAmPm = p.find(".clockpicker-buttons-am-pm"), this.currentPlacementClass = r.placement, this.raiseCallback = function () { n.apply(x, arguments) }, r.twelvehour && (t(this.buttonsAmPm).css("display", "flex"), t('<a class="btn-am">AM</a>').on("click", function () { x.amOrPm = "AM", t(this).removeClass("text-white-50"), t(".btn-pm").addClass("text-white-50"), r.ampmSubmit && setTimeout(function () { x.done() }, duration / 2) }).appendTo(this.buttonsAmPm), t('<a class="btn-pm text-white-50">PM</a>').on("click", function () { x.amOrPm = "PM", t(this).removeClass("text-white-50"), t(".btn-am").addClass("text-white-50"), r.ampmSubmit && setTimeout(function () { x.done() }, duration / 2) }).appendTo(this.buttonsAmPm)), r.autoclose || (V.append('<button type="button" class="btn btn-sm btn-outline-primary">' + r.canceltext + "</button>").on("click", function () { t(this).closest(".clockpicker-popover").hide() }), V.css("display", "flex").append('<button type="button" class="btn btn-sm btn-outline-primary">' + r.donetext + "</button>").click(t.proxy(this.done, this))), !/^(top|bottom)/.test(r.placement) || "top" !== r.align && "bottom" !== r.align || (r.align = "left"), "left" !== r.placement && "right" !== r.placement || "left" !== r.align && "right" !== r.align || (r.align = "top"), p.addClass(r.placement), p.addClass("clockpicker-align-" + r.align), this.spanHours.click(t.proxy(this.toggleView, this, "hours")), this.spanMinutes.click(t.proxy(this.toggleView, this, "minutes")), r.addonOnly || A.on("focus.clockpicker click.clockpicker", t.proxy(this.show, this)), T.on("click.clockpicker", t.proxy(this.toggle, this)); var S, D, I, E, O = t('<div class="clockpicker-tick"></div>'); if (r.twelvehour) for (S = 0; S < 12; S += r.hourstep)D = O.clone(), I = S / 6 * Math.PI, E = w, D.css("font-size", "120%"), D.css({ left: g + Math.sin(I) * E - C, top: g - Math.cos(I) * E - C }), D.html(0 === S ? 12 : S), d.append(D), D.on(f, c); else for (S = 0; S < 24; S += r.hourstep) { var L = !1; if (r.disabledhours && t.inArray(S, r.disabledhours) != -1) var L = !0; D = O.clone(), I = S / 6 * Math.PI; var z = S > 0 && S < 13; E = z ? y : w, D.css({ left: g + Math.sin(I) * E - C, top: g - Math.cos(I) * E - C }), z && D.css("font-size", "120%"), L && D.addClass("disabled"), D.html(0 === S ? "00" : S), d.append(D), L || D.on(f, c) } var B = Math.max(r.minutestep, 5); for (S = 0; S < 60; S += B)D = O.clone(), I = S / 30 * Math.PI, D.css({ left: g + Math.sin(I) * w - C, top: g - Math.cos(I) * w - C }), D.css("font-size", "120%"), D.html(i(S)), v.append(D), D.on(f, c); if (u.on(f, function (e) { 0 === t(e.target).closest(".clockpicker-tick").length && c(e, !0) }), l) { var N = p.find(".clockpicker-canvas"), j = e("svg"); j.setAttribute("class", "clockpicker-svg"), j.setAttribute("width", diameter), j.setAttribute("height", diameter); var U = e("g"); U.setAttribute("transform", "translate(" + g + "," + g + ")"); var W = e("circle"); W.setAttribute("class", "clockpicker-canvas-bearing"), W.setAttribute("cx", 0), W.setAttribute("cy", 0), W.setAttribute("r", 3); var R = e("line"); R.setAttribute("x1", 0), R.setAttribute("y1", 0); var X = e("circle"); X.setAttribute("class", "clockpicker-canvas-bg"), X.setAttribute("r", C); var Y = e("circle"); Y.setAttribute("class", "clockpicker-canvas-fg"), Y.setAttribute("r", 3.5), U.appendChild(R), U.appendChild(X), U.appendChild(Y), U.appendChild(W), j.appendChild(U), N.append(j), this.hand = R, this.bg = X, this.fg = Y, this.bearing = W, this.g = U, this.canvas = N } this.raiseCallback(this.options.init, "init") } function n(t, e) { if (t && "function" == typeof t && this.element) { var i = this.getTime() || null; t.call(this.element, i) } e && this.element.trigger("clockpicker." + e || "NoName") } function r(t, e, i) { var s = e.outerHeight(), o = t.outerHeight(), n = t.offset().top, r = t.offset().top + o, a = n - t[0].getBoundingClientRect().top, c = a + document.documentElement.clientHeight, h = n - s >= a, p = r + s <= c; if ("top" === i) { if (h) return "top"; if (p) return "bottom" } else { if (p) return "bottom"; if (h) return "top" } return "viewport-top" } var a, c = t(window), h = t(document), p = "http://www.w3.org/2000/svg", l = "SVGAngle" in window && function () { var t, e = document.createElement("div"); return e.innerHTML = "<svg/>", t = (e.firstChild && e.firstChild.namespaceURI) == p, e.innerHTML = "", t }(), u = function () { var t = document.createElement("div").style; return "transition" in t || "WebkitTransition" in t || "MozTransition" in t || "msTransition" in t || "OTransition" in t }(), d = "ontouchstart" in window, f = "mousedown" + (d ? " touchstart" : ""), k = "mousemove.clockpicker" + (d ? " touchmove.clockpicker" : ""), m = "mouseup.clockpicker" + (d ? " touchend.clockpicker" : ""), v = navigator.vibrate ? "vibrate" : navigator.webkitVibrate ? "webkitVibrate" : null, b = 0, g = 100, w = 80, y = 54, C = 13; diameter = 2 * g, duration = u ? 350 : 1; var M = ['<div class="popover clockpicker-popover">', '<div class="arrow"></div>', '<div class="popover-header">', '<span class="clockpicker-span-hours"></span>', ":", '<span class="clockpicker-span-minutes text-white-50"></span>', '<span class="clockpicker-buttons-am-pm"></span>', "</div>", '<div class="popover-body">', '<div class="clockpicker-plate">', '<div class="clockpicker-canvas"></div>', '<div class="clockpicker-dial clockpicker-hours"></div>', '<div class="clockpicker-dial clockpicker-minutes clockpicker-dial-out"></div>', "</div>", '<div class="clockpicker-close-block justify-content-end"></div>', "</div>", "</div>"].join(""); o.prototype.parseStep = function (t, e) { return e % t === 0 ? t : 1 }, o.DEFAULTS = { default: "", fromnow: 0, placement: "bottom", align: "left", donetext: "OK", canceltext: "Cancel", autoclose: !1, twelvehour: !1, vibrate: !0, hourstep: 1, minutestep: 1, ampmSubmit: !1, addonOnly: !1, disabledhours: null }, o.prototype.toggle = function () { this[this.isShown ? "hide" : "show"]() }, o.prototype.updatePlacementClass = function (t) { this.currentPlacementClass && this.popover.removeClass(this.currentPlacementClass), t && this.popover.addClass(t), this.currentPlacementClass = t }, o.prototype.locate = function () { var t = this.element, e = this.popover, i = t.offset(), s = t.outerWidth(), o = t.outerHeight(), n = this.options.placement, a = this.options.align, c = {}; if ("top-adaptive" === n || "bottom-adaptive" === n) { var h = n.substr(0, n.indexOf("-")); n = r(t, e, h), this.updatePlacementClass("viewport-top" !== n ? n : "") } switch (e.show(), n) { case "bottom": c.top = i.top + o; break; case "right": c.left = i.left + s; break; case "top": c.top = i.top - e.outerHeight(); break; case "left": c.left = i.left - e.outerWidth(); break; case "viewport-top": c.top = i.top - t[0].getBoundingClientRect().top }switch (a) { case "left": c.left = i.left; break; case "right": c.left = i.left + s - e.outerWidth(); break; case "top": c.top = i.top; break; case "bottom": c.top = i.top + o - e.outerHeight() }e.css(c) }, o.prototype.parseInputValue = function () { var t = this.input.prop("value") || this.options.default || ""; if ("now" === t && (t = new Date(+new Date + this.options.fromnow)), t instanceof Date && (t = t.getHours() + ":" + t.getMinutes()), t = t.split(":"), this.hours = +t[0] || 0, this.minutes = +(t[1] + "").replace(/\D/g, "") || 0, this.hours = Math.round(this.hours / this.options.hourstep) * this.options.hourstep, this.minutes = Math.round(this.minutes / this.options.minutestep) * this.options.minutestep, this.options.twelvehour) { var e = (t[1] + "").replace(/\d+/g, "").toLowerCase(); this.amOrPm = this.hours > 12 || "am" === e ? "AM" : "PM" } }, o.prototype.show = function (e) { if (!this.isShown) { this.raiseCallback(this.options.beforeShow, "beforeShow"); var s = this; this.isAppended || (a = t(document.body).append(this.popover), c.on("resize.clockpicker" + this.id, function () { s.isShown && s.locate() }), this.isAppended = !0), this.parseInputValue(), this.spanHours.html(i(this.hours)), this.spanMinutes.html(i(this.minutes)), this.toggleView("hours"), this.locate(), this.isShown = !0, h.on("click.clockpicker." + this.id + " focusin.clockpicker." + this.id, function (e) { var i = t(e.target); 0 === i.closest(s.popover).length && 0 === i.closest(s.addon).length && 0 === i.closest(s.input).length && s.hide() }), h.on("keyup.clockpicker." + this.id, function (t) { 27 === t.keyCode && s.hide() }), this.raiseCallback(this.options.afterShow, "afterShow") } }, o.prototype.hide = function () { this.raiseCallback(this.options.beforeHide, "beforeHide"), this.isShown = !1, h.off("click.clockpicker." + this.id + " focusin.clockpicker." + this.id), h.off("keyup.clockpicker." + this.id), this.popover.hide(), this.raiseCallback(this.options.afterHide, "afterHide") }, o.prototype.toggleView = function (e, i) { var s = !1; "minutes" === e && "visible" === t(this.hoursView).css("visibility") && (this.raiseCallback(this.options.beforeHourSelect, "beforeHourSelect"), s = !0); var o = "hours" === e, n = o ? this.hoursView : this.minutesView, r = o ? this.minutesView : this.hoursView; this.currentView = e, this.spanHours.toggleClass("text-white-50", !o), this.spanMinutes.toggleClass("text-white-50", o), r.addClass("clockpicker-dial-out"), n.css("visibility", "visible").removeClass("clockpicker-dial-out"), this.resetClock(i), clearTimeout(this.toggleViewTimer), this.toggleViewTimer = setTimeout(function () { r.css("visibility", "hidden") }, duration), s && this.raiseCallback(this.options.afterHourSelect, "afterHourSelect") }, o.prototype.resetClock = function (t) { var e = this.currentView, i = this[e], s = "hours" === e, o = Math.PI / (s ? 6 : 30), n = i * o, r = s && i > 0 && i < 13 ? y : w, a = Math.sin(n) * r, c = -Math.cos(n) * r, h = this; l && t ? (h.canvas.addClass("clockpicker-canvas-out"), setTimeout(function () { h.canvas.removeClass("clockpicker-canvas-out"), h.setHand(a, c) }, t)) : this.setHand(a, c) }, o.prototype.setHand = function (e, s, o) { var n, r, a = Math.atan2(e, -s), c = "hours" === this.currentView, h = Math.sqrt(e * e + s * s), p = this.options, u = c && h < (w + y) / 2, d = u ? y : w; if (n = c ? p.hourstep / 6 * Math.PI : p.minutestep / 30 * Math.PI, p.twelvehour && (d = w), a < 0 && (a = 2 * Math.PI + a), r = Math.round(a / n), a = r * n, c) { if (r *= p.hourstep, p.twelvehour || !u != r > 0 || (r += 12), p.twelvehour && 0 === r && (r = 12), 24 === r && (r = 0), o && !p.twelvehour && p.disabledhours && t.inArray(r, p.disabledhours) != -1) return } else r *= p.minutestep, 60 === r && (r = 0); if (this[this.currentView] !== r && v && this.options.vibrate && (this.vibrateTimer || (navigator[v](10), this.vibrateTimer = setTimeout(t.proxy(function () { this.vibrateTimer = null }, this), 100))), this[this.currentView] = r, this[c ? "spanHours" : "spanMinutes"].html(i(r)), !l) return void this[c ? "hoursView" : "minutesView"].find(".clockpicker-tick").each(function () { var e = t(this); e.toggleClass("active", r === +e.html()) }); o || !c && r % 5 ? (this.g.insertBefore(this.hand, this.bearing), this.g.insertBefore(this.bg, this.fg), this.bg.setAttribute("class", "clockpicker-canvas-bg clockpicker-canvas-bg-trans")) : (this.g.insertBefore(this.hand, this.bg), this.g.insertBefore(this.fg, this.bg), this.bg.setAttribute("class", "clockpicker-canvas-bg")); var f = Math.sin(a) * d, k = -Math.cos(a) * d; this.hand.setAttribute("x2", f), this.hand.setAttribute("y2", k), this.bg.setAttribute("cx", f), this.bg.setAttribute("cy", k), this.fg.setAttribute("cx", f), this.fg.setAttribute("cy", k) }, o.prototype.getTime = function (t) { this.parseInputValue(); var e = this.hours; this.options.twelvehour && e < 12 && "PM" === this.amOrPm && (e += 12); var i = new Date; return i.setMinutes(this.minutes), i.setHours(e), i.setSeconds(0), t && t.apply(this.element, i) || i }, o.prototype.done = function () { this.raiseCallback(this.options.beforeDone, "beforeDone"), this.hide(); var t = this.input.prop("value"), e = this.hours, s = ":" + i(this.minutes); this.isHTML5 && this.options.twelvehour && (this.hours < 12 && "PM" === this.amOrPm && (e += 12), 12 === this.hours && "AM" === this.amOrPm && (e = 0)), s = i(e) + s, !this.isHTML5 && this.options.twelvehour && (s += this.amOrPm), this.input.prop("value", s), s !== t && (this.input.trigger("change"), this.isInput || this.element.trigger("change")), this.options.autoclose && this.input.trigger("blur"), this.raiseCallback(this.options.afterDone, "afterDone") }, o.prototype.remove = function () { this.element.removeData("clockpicker"), this.input.off("focus.clockpicker click.clockpicker"), this.addon.off("click.clockpicker"), this.isShown && this.hide(), this.isAppended && (c.off("resize.clockpicker" + this.id), this.popover.remove()) }, t.fn.clockpicker = function (e) { function i() { var i = t(this), n = i.data("clockpicker"); if (n) { if ("function" == typeof n[e]) return n[e].apply(n, s) } else { var r = t.extend({}, o.DEFAULTS, i.data(), "object" == typeof e && e); i.data("clockpicker", new o(i, r)) } } var s = Array.prototype.slice.call(arguments, 1); if (1 == this.length) { var n = i.apply(this[0]); return void 0 !== n ? n : this } return this.each(i) } })(jQuery);