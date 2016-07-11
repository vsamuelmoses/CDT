app.config(['$httpProvider', ($httpProvider) => {

    if (!$httpProvider.defaults.headers.get) {
        $httpProvider.defaults.headers.common = {};
    }
    $httpProvider.defaults.headers.common["Cache-Control"] = "no-cache";
    $httpProvider.defaults.headers.common.Pragma = "no-cache";
    $httpProvider.defaults.headers.common["If-Modified-Since"] = "0";
    $httpProvider.defaults.headers.common["Accept"] = "*/*";

    $httpProvider.interceptors.push(($q, $rootScope) => {
        return {
            request: (config) => {
                return config;
            },

            response: result => { return result; },

            responseError: rejection => {
                if (rejection.status == 402) {
                    $rootScope.state.go("dashboard");
                    return $q.reject(rejection);
                }
                if (rejection.status === 0) {
                    $rootScope.$broadcast("serverError", { message: "Сonnection with host is lost" });
                    return $q.defer();
                }
                return $q.reject(rejection);
            }
        }
    });
}]);  