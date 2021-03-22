$(function () {
    function loadDfp() {
        if (typeof dfp !== "undefined") {
            dfp.doFpt(window.document)
        }
        else {
            setTimeout(loadDfp, 1000);
        }
    }

    var correlationId = new URLSearchParams(SETTINGS.remoteResource.split('?')[1]).get('correlationId');
    var dfpFptUrl = "https://fpt.dfp.microsoft.com/mdt.js?session_id=" + correlationId + "&instanceId=96a3f126-5b1a-4967-96e2-3e968db955f5";

    $.getScript(dfpFptUrl, loadDfp)
});