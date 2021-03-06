function CustomMarker(latlng, map, args) {
    this.latlng = latlng;
    this.args = args;
    this.setMap(map);
}
var thumbsList = "";
var PicsList = "";
var modalTitle = "";
var reviewsObj;
CustomMarker.prototype = new google.maps.OverlayView();

CustomMarker.prototype.draw = function () {

    var self = this;

    var div = this.div;

    if (!div) {

        div = this.div = document.createElement('div');

        div.className = 'marker';

        div.style.position = 'absolute';
        div.style.cursor = 'pointer';
        div.style.width = '20px';
        div.style.height = '20px';
        //div.style.background = 'blue';
        if (typeof (self.args.marker_id) !== 'undefined') {
            div.innerHTML = '<a class="btn-floating btn-large red" style="top:10px"><i class="' + self.args.marker_id + '"></i></a>';
        }

        //if (typeof(self.args.marker_id) !== 'undefined') {
        //	div.dataset.marker_id = self.args.marker_id;
        //}

        google.maps.event.addDomListener(div, "click", function (event) {

            if ($('#panel').css('top') != "60px") {
                $('#panel-body').toggle();
                $('#panel').css({ top: '60px' });
                $('#panel').css({ width: '100px' });
            }
            service.getDetails(self.args.request, function (details, status) {
                thumbsList = "";
                PicsList = "";
                var content = "<div class='scrollFix'><div class='detailTitle'>" + details.name + "</div>";
                if (details.formatted_address != undefined) {
                    content += details.formatted_address + "<br />";
                }
                if (details.website != undefined) {
                    content += '<a href="' + details.website + '">' + details.website + '</a>' + "<br />";
                }
                if (details.rating != undefined) {
                    content += details.rating + "<br />";
                }
                if (details.formatted_phone_number != undefined) {
                    content += '<a href="tel:' + details.international_phone_number + '">' + details.formatted_phone_number + '</a>';
                }
                if (details.photos != undefined) {
                    modalTitle = details.name;
                    for (var i = 0; i < details.photos.length; i++) {
                        thumbsList += details.photos[i].getUrl({ 'maxWidth': 30, 'maxHeight': 30 }) + ",";
                        PicsList += details.photos[i].getUrl({ 'maxWidth': 300, 'maxHeight': 300 }) + ",";
                    }
                    content += "<a class='btn-floating btn pink lighten-3' style='float:right;margin-left: 10px;' onClick='imageDetayClick()'><i class='mdi-image-photo-camera'></i></a>";
                }
                if (details.reviews != undefined) {
                    modalTitle = details.name;
                    reviewsObj = details.reviews
                    content += "<a class='btn-floating btn pink lighten-3' style='float:right' onClick='reviewDetayClick()'><i class='mdi-editor-insert-comment'></i></a>";
                }
                content += "</div>";

                infoBubble.setContent(content);
                infoBubble.open(map, self.args.marker);
            });
        });
        var panes = this.getPanes();
        panes.overlayImage.appendChild(div);
    }

    var point = this.getProjection().fromLatLngToDivPixel(this.latlng);

    if (point) {
        div.style.left = (point.x - 10) + 'px';
        div.style.top = (point.y - 20) + 'px';
    }
};

CustomMarker.prototype.remove = function () {
    if (this.div) {
        this.div.parentNode.removeChild(this.div);
        this.div = null;
    }
};

CustomMarker.prototype.getPosition = function () {
    return this.latlng;
};


