$(function() {
    function loadDfp() {
        if (typeof dfp !== "undefined") {
            dfp.doFpt(window.document)
        } else {
            setTimeout(loadDfp, 1000);
        }
    }

    var correlationId = new URLSearchParams(SETTINGS.remoteResource.split('?')[1]).get('correlationId');
    var dfpFptUrl = "https://fpt.dfp.microsoft.com/mdt.js?session_id=" + correlationId + "&instanceId=<YOUR-DFP-INSTANCE-ID>";

    $.getScript(dfpFptUrl, loadDfp)
});