const getCoordinates = (dotnetObj, successMethodName, errorMethodName) => {
    if (navigator && navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
            (position) => {
                const { longitude, latitude } = position.coords;
                dotnetObj.invokeMethodAsync(
                    successMethodName, longitude, latitude);
            },
            (error) => {
                const { code, message } = error;
                dotnetObj.invokeMethodAsync(
                    errorMethodName, code, message);
            });
    }
};

window.site = {
    getCoordinates
};
