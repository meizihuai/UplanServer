// 百度地图API功能
var map = new BMap.Map("allmap");
var top_left_control = null;
var top_left_navigation = null;
var top_right_navigation = null;

function iniMap(id,centerLng,centerLat,viewSize) {
    map = new BMap.Map(id);
    var point = new BMap.Point(centerLng, centerLat);
    top_left_control = new BMap.ScaleControl({
        anchor: BMAP_ANCHOR_TOP_LEFT
    }); // 左上角，添加比例尺
    top_left_navigation = new BMap.NavigationControl(); //左上角，添加默认缩放平移控件
    top_right_navigation = new BMap.NavigationControl({
        anchor: BMAP_ANCHOR_TOP_RIGHT,
        type: BMAP_NAVIGATION_CONTROL_SMALL
    }); //右上角，仅包含平移和缩放按钮
    add_control();
    map.centerAndZoom(point, viewSize);
    map.enableScrollWheelZoom(true);
    map.addControl(new BMap.MapTypeControl());
    var stCtrl = new BMap.PanoramaControl(); //构造全景控件
    stCtrl.setOffset(new BMap.Size(0, 50));
    map.addControl(stCtrl); //添加全景控件
}


// 编写自定义函数,创建标注
///addpoint(113.456,23.456,"hasaki");



function add_control() {
    map.addControl(top_left_control);
    map.addControl(top_left_navigation);
    //map.addControl(top_right_navigation);
}
//移除控件和比例尺
function delete_control() {
    map.removeControl(top_left_control);
    map.removeControl(top_left_navigation);
    map.removeControl(top_right_navigation);
}

function setcenter(lng, lat) {
    point = new BMap.Point(lng, lat);
    map.centerAndZoom(point, 15);
}

function setcenter3(lng, lat, size) {
    point = new BMap.Point(lng, lat);
    map.centerAndZoom(point, size);
}

function addpoint(lng, lat, text) {
    point = new BMap.Point(lng, lat);
    var marker = new BMap.Marker(point);
    map.addOverlay(marker);
    marker.setAnimation(BMAP_ANIMATION_BOUNCE);
    var label = new BMap.Label(text, {
        offset: new BMap.Size(20, -10)
    });
    marker.setLabel(label);
}
//addBz(113.234,23.456,"梅子怀");
function addBz(lng, lat, text) {
    var point = new BMap.Point(lng, lat);
    addMarker(point, text);
}

function addMarker(point, text) {
    var marker = new BMap.Marker(point);
    var label = new BMap.Label(text, {
        offset: new BMap.Size(20, -10)
    });
    marker.setLabel(label);
    var markerMenu = new BMap.ContextMenu();
    markerMenu.addItem(new BMap.MenuItem('历史数据回放', function () {
        window.external.showLiShi(text);
    }));
    markerMenu.addItem(new BMap.MenuItem('实时监测', function () {
        window.external.showShiShi(text);
    }));
    marker.addContextMenu(markerMenu);
    map.addOverlay(marker);
}

function addNewIcoPoint(lng, lat, text, icoUrl) {
    // alert(icoUrl);
    var pt = new BMap.Point(lng, lat);
    var myIcon = new BMap.Icon(icoUrl, new BMap.Size(90, 130));
    var marker2 = new BMap.Marker(pt, {
        icon: myIcon
    }); // 创建标注
    var label = new BMap.Label(text, {
        offset: new BMap.Size(20, -10)
    });
    marker2.setLabel(label);

    // var markerMenu = new BMap.ContextMenu();
    // markerMenu.addItem(new BMap.MenuItem('切换至云平台', function () { window.external.SwitchOut(text); }));
    // markerMenu.addItem(new BMap.MenuItem('切换至监测网', function () { window.external.SwitchIn(text); }));
    // marker2.addContextMenu(markerMenu);

    map.addOverlay(marker2); // 将标注添加到地图中
}

function addNewIco(lng, lat, icoUrl) {
    var pt = new BMap.Point(lng, lat);
    var myIcon = new BMap.Icon(icoUrl, new BMap.Size(90, 130));
    var marker2 = new BMap.Marker(pt, {
        icon: myIcon
    }); // 创建标注
    map.addOverlay(marker2);
}
// addFreqGisPoint(113.345,23.345,"2018-11-18 20:11:00",false,18,113.346,23.346,true,"blue",4,5,10,0.5)
// window.onload = function () {
//   var arr = [{
//     "lng": "113.312497782774",
//     "lat": "23.1274426966053",
//     "info": "2018-11-18 15:36:24",
//     "isShowPoint":false,
//     "isCenter": "False",
//     "centerSize": null,
//     "oldlng": "113.312485539547",
//     "oldlat": "23.1274269058585",
//     "isMakePolyLine":true,
//     "polyLineColor": "blue",
//     "lineId": "4",
//     "msgId": "114677",
//     "strokeWeight": "10",
//     "strokeOpacity": "0.5"
//   },{
//     "lng": "113.312597782774",
//     "lat": "23.1284426966053",
//     "info": "2018-11-18 15:36:24",
//     "isShowPoint":false,
//     "isCenter": "False",
//     "centerSize": null,
//     "oldlng": "113.312485539547",
//     "oldlat": "23.1274269058585",
//     "isMakePolyLine": true,
//     "polyLineColor": "blue",
//     "lineId": "4",
//     "msgId": "114677",
//     "strokeWeight": "10",
//     "strokeOpacity": "0.5"
//   } ];

//   var js =JSON.stringify(arr);
//   addFreqGisPointList(js)

// }
function addFreqGisPointList(arrstr) {
    if (arrstr == null) return;
    var list = JSON.parse(arrstr);
    for (var i = 0; i < list.length; i++) {
        var itm = list[i];
        addFreqGisPoint(itm.lng, itm.lat, itm.info, itm.isShowPoint, itm.isCenter, itm.centerSize, itm.oldlng, itm.oldlat,
            itm.isMakePolyLine, itm.polyLineColor, itm.lineId,
            itm.msgId, itm.strokeWeight, itm.strokeOpacity);
    }
}
//  addFreqGisPoint(113.345,23.345,"2018-11-18 20:11:00",false,true,18,113.346,23.346,true,"blue",4,5,10,0.5);
// window.onload=function(){
//   var ps=[];
//   for(var i=0;i<100;i++){
//     var k=i*0.0001;
//     var itm={
//       x:113.345+k,
//       y:23.345+k
//     };
//     ps.push(itm);
//     addFreqGisPoint(113.345+k,23.345+k,"2018-11-18 20:11:00",false,true,18,113.346+k,23.346+k,true,"blue",4,5,10,0.5);
//   }
//   var js=JSON.stringify(ps);
//   addHaiLiangPoints(js);
// }
function addFreqGisPoint(lng, lat, info, isShowPoint, isCenter, centerSize, oldlng, oldlat, isMakePolyLine,
    polyLineColor, lineId, msgId, strokeWeight, strokeOpacity) {
    if (polyLineColor == "") {
        polyLineColor = "blue"
    }
    if (isCenter) {
        setcenter3(lng, lat, centerSize);
    }

    var point = new BMap.Point(lng, lat);
    if (isShowPoint) {
        var marker = new BMap.Marker(point);
        if (info != "") {
            var label = new BMap.Label(info, {
                offset: new BMap.Size(20, -10)
            });
            marker.setLabel(label);
        }

        var markerMenu = new BMap.ContextMenu();
        markerMenu.addItem(new BMap.MenuItem('查看历史频谱', function () {
            window.external.ShowHisFreqGis(lng, lat, lineId, msgId);
        }));
        marker.addContextMenu(markerMenu);
        map.addOverlay(marker);
    }
    //alert(isMakePolyLine);
    if (isMakePolyLine) {
        var polyline = new BMap.Polyline([
            new BMap.Point(oldlng, oldlat),
            new BMap.Point(lng, lat)
        ], {
                strokeColor: polyLineColor,
                strokeWeight: strokeWeight,
                strokeOpacity: strokeOpacity
            }); //创建折线
        map.addOverlay(polyline);
        // alert("polyline");
    }
    // if (document.createElement('canvas').getContext) {
    //    addHaiLiangPoint(lng, lat);
    // }
}
// window.onload=function(){
//   setcenter(113.456,23.456);
//   var obj=[{
//     x:113.456,
//     y:23.456,
//     lineId:"123",
//     msgId:"456"
//   },{
//     x:113.457,
//     y:23.457,
//     lineId:"2374",
//     msgId:"45636"
//   }]
//   addHaiLiangPoints(JSON.stringify(obj));
// }
function addHaiLiangPoints(data) {
    if (!document.createElement('canvas').getContext) {
        return;
    }
    var points = [];
    var obj = JSON.parse(data);
    for (var i = 0; i < obj.length; i++) {
        var itm = obj[i];
        var p = new BMap.Point(itm.x, itm.y);
        p.data = JSON.stringify(itm);
        points.push(p);
    }
    var options = {
        size: BMAP_POINT_SIZE_BIG,
        shape: BMAP_POINT_SHAPE_CIRCLE,
        color: '#d340c3'
    }

    var pointCollection = new BMap.PointCollection(points, options); // 初始化PointCollection
    pointCollection.addEventListener('click', function (e) {
        //alert('单击点的坐标为：' + e.point.lng + ',' + e.point.lat);  // 监听点击事件
        //var obj=e.point.data;
        // alert( obj.lineId);
        var str = e.point.data;
        window.external.ShowHisFreqGisByJson(str);

    });
    map.addOverlay(pointCollection); // 添加Overlay

    delete points;
    delete pointCollection;
}

function addHaiLiangPoint(lng, lat) {
    var points = [
        new BMap.Point(lng, lat)
    ];
    var options = {
        size: BMAP_POINT_SIZE_BIG,
        shape: BMAP_POINT_SHAPE_CIRCLE,
        color: '#d340c3'
    }

    var pointCollection = new BMap.PointCollection(points, options); // 初始化PointCollection
    // pointCollection.addEventListener('click', function (e) {
    //   alert('单击点的坐标为：' + e.point.lng + ',' + e.point.lat);  // 监听点击事件
    // });
    map.addOverlay(pointCollection); // 添加Overlay

    delete points;
    delete pointCollection;
}

function addPolyline(lng1, lat1, lng2, lat2, flagsetcenter, color, icoUrl, lineId) {
    if (color == "") {
        color = "blue"
    }
    // color="'"+color+"'";
    var polyline = new BMap.Polyline([
        new BMap.Point(lng1, lat1),
        new BMap.Point(lng2, lat2)
    ], {
            strokeColor: color,
            strokeWeight: 10,
            strokeOpacity: 0.5
        }); //创建折线
    if (flagsetcenter) {
        setcenter(lng1, lat1);
    }
    map.addOverlay(polyline); //增加折线
    var lng = lng1;
    var lat = lat1;
    var pt = new BMap.Point(lng, lat);
    var myIcon = new BMap.Icon(icoUrl, new BMap.Size(30, 30));
    var marker2 = new BMap.Marker(pt, {
        icon: myIcon
    }); // 创建标注
    var markerMenu = new BMap.ContextMenu();
    markerMenu.addItem(new BMap.MenuItem('查看历史频谱', function () {
        window.external.ShowHisFreqGis(lng, lat, lineId);
    }));
    marker2.addContextMenu(markerMenu);
    map.addOverlay(marker2);
}

function cleanall() {
    map.clearOverlays();
}

function deletePoint(info) {
    var allOverlay = map.getOverlays();
    for (var i = 0; i < allOverlay.length; i++) {
        try {
            if (allOverlay[i].getLabel().content == info) {
                map.removeOverlay(allOverlay[i]);
                return false;
            }
        } catch (e) {

        }
    }
}
//addBz(113.256,23.456,"meizihuai");
//setcenter2(113.256,23.456);
//addNewIcoPoint(113.256,23.456,"meizihuai","http://123.207.31.37:8082/bmapico/tzbq.png");
function setcenter2(lng, lat) {
    var bs = map.getBounds(); //获取可视区域
    var bssw = bs.getSouthWest(); //可视区域左下角
    var bsne = bs.getNorthEast(); //可视区域右上角
    //var resutl=bssw.lng + "," + bssw.lat + "," + bsne.lng + "," + bsne.lat;
    var halfwidth = Math.abs(bsne.lng - bssw.lng) / 2;
    // var halfheight = Math.abs(bsne.lat - bssw.lat) / 2;
    setcenter(lng - halfwidth + 0.01, lat);
}
//showWindowMsg(116.404,39.915,"天安门坐落在中国北京市中心,故宫的南侧,与天安门广场隔长安街相望,是清朝皇城的大门...",true);
function showWindowMsg(lng, lat, msg, iscenter) {
    var sContent = msg;
    var point = new BMap.Point(lng, lat);
    if (iscenter == true) {
        map.centerAndZoom(point, 15);
    }
    var infoWindow = new BMap.InfoWindow(sContent); // 创建信息窗口对象
    map.openInfoWindow(infoWindow, point); //开启信息窗口
}