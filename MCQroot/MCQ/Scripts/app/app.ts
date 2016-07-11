var mcq = angular.module("mcq", []);
mcq.controller("homeController", Mcq.Home.homeController);
mcq.controller("topicsMenuController", Mcq.TopicsMenu.topicsMenuController);
mcq.controller("navLoginController", Mcq.Home.navLoginController);
mcq.controller("registerController", Mcq.Home.registerController);
mcq.controller("loginController", Mcq.Home.loginController);
mcq.controller("topicSummaryController", Mcq.Home.topicSummaryController);
mcq.controller("dashboardController", Mcq.Home.dashboardController);
mcq.controller("quizController", Mcq.Quiz.quizController);
mcq.controller("questionController", Mcq.Quiz.questionController);
mcq.controller("paymentController", Mcq.Home.paymentController);
mcq.controller("accountSettingsController", Mcq.Home.accountSettingsController);
mcq.controller("resetPasswordController", Mcq.Home.resetPasswordController);

mcq.service("httpHelpers", Mcq.Helpers.httpHelpersFactory);
mcq.service("accountService", Mcq.Home.accountServiceFactory);
mcq.service("topicService", Mcq.Home.topicServiceFactory);
mcq.service("quizService", Mcq.Quiz.quizServiceFactory);
mcq.service("paymentService", Mcq.Home.paymentServiceFactory);

var app = angular.module("app", [
    "ui.router", "ui.bootstrap", "angular-growl", "n3-pie-chart", 'angular-loading-bar', 'ui.slider', "mcq", 'angulike', 'ngCookies', 'tc.chartjs']);

app.config(($stateProvider: ng.ui.IStateProvider, $urlRouterProvider: ng.ui.IUrlRouterProvider, cfpLoadingBarProvider, $cookiesProvider) => {
    $urlRouterProvider.otherwise("/dashboard");
    $stateProvider.state("home", {
        url: "/home",
        templateUrl: window["urls"].templates.home,
        controller: "homeController"
    }).state("topicSummary", {
        url: "/topicSummary",
        templateUrl: window["urls"].templates.topicSummary,
        controller: "topicSummaryController"
    }).state("dashboard", {
        url: "/dashboard",
        templateUrl: window["urls"].templates.dashboard,
        controller: "dashboardController"
    }).state("quiz", {
        url: "/quiz",
        templateUrl: window["urls"].templates.quiz,
        controller: "quizController"
    }).state("payment", {
        url: "/payment",
        templateUrl: window["urls"].templates.payment,
        controller: "paymentController"
    }).state("accountSettings", {
        url: "/accountSettings",
        templateUrl: window["urls"].templates.accountSettings,
        controller: "accountSettingsController"
    }).state("resetPassword", {
        url: "/resetPassword/:userId/:code",
        templateUrl: window["urls"].templates.resetPassword,
        controller: "resetPasswordController"
    });
    cfpLoadingBarProvider.includeSpinner = false;

});


app.run(['$rootScope', 'growl', 'accountService', '$window', "$state", ($rootScope, growl, accountService: Mcq.Home.AccountService, $window, $state) => {
    accountService.getCurrentUser().then((result) => {
        if (result.isSucceed) {
            result.data.userName = result.data.firstName + " " + result.data.lastName;
        }

        $rootScope.loggedUser = result.data;
        $rootScope.navVisible = true;
        //if ($window.outerWidth <= 768) {
        //    $rootScope.navVisible =false;
        //}
        $rootScope.toggleNav = () => {
            if ($window.outerWidth <= 768) {
                //$rootScope.navVisible = !$rootScope.navVisible;

                $('#side-menu')['collapse']('hide');
            }

        };
        $rootScope.state = $state;

        $rootScope.$watch(() => {
            return $rootScope.loggedUser;
        },(newValue, oldValue) => {
                if (newValue) {
                    $state.go("dashboard");
                    $rootScope.$broadcast("userSignedIn");
                }
                if (!newValue) {
                    $state.go("dashboard");
                }
            });

        //$rootScope.$on('$stateChangeEnd', function (ev, to, toParams, from, fromParams) {
        //    alert(1);
        //    if (!$rootScope.loggedUser) {
        //        $state.go("home");
        //    }
        //});
    });

    $rootScope.facebookAppId = '463826557150604';
    $window.fbAsyncInit = function () {
        window["FB"].init({
            appId: '463826557150604',

            status: true,
            cookie: true,
            xfbml: true,
            version: 'v2.4'
        });

    };

    (function (d) {
        // load the Facebook javascript SDK

        var js,
            id = 'facebook-jssdk',
            ref = d.getElementsByTagName('script')[0];

        if (d.getElementById(id)) {
            return;
        }

        js = d.createElement('script');
        js.id = id;
        js.async = true;
        js.src = "https://connect.facebook.net/en_US/all.js";

        ref.parentNode.insertBefore(js, ref);

    } (document));
   
}]);

app.config(['growlProvider', (growlProvider) => {
    growlProvider.globalPosition('top-center');
    growlProvider.globalDisableCountDown(true);
    growlProvider.globalTimeToLive(5000);
}]);


mcq.service('modalService', ['$modal',
    function ($uibModal) {

        var modalDefaults = {
            backdrop: true,
            keyboard: true,
            modalFade: true,
            templateUrl: 'Template'
        };

        var modalOptions = {
            closeButtonText: 'Close',
            actionButtonText: 'OK',
            headerText: 'Proceed?',
            bodyText: 'Perform this action?'
        };

        this.showModal = function (customModalDefaults, customModalOptions) {
            if (!customModalDefaults) customModalDefaults = {};
            customModalDefaults.backdrop = 'static';
            return this.show(customModalDefaults, customModalOptions);
        };

        this.show = function (customModalDefaults, customModalOptions) {
            //Create temp objects to work with since we're in a singleton service
            var tempModalDefaults = {};
            var tempModalOptions = {};

            //Map angular-ui modal custom defaults to modal defaults defined in service
            angular.extend(tempModalDefaults, modalDefaults, customModalDefaults);

            //Map modal.html $scope custom properties to defaults defined in service
            angular.extend(tempModalOptions, modalOptions, customModalOptions);

            if (!tempModalDefaults["controller"]) {
                tempModalDefaults["controller"] = function ($scope, $uibModalInstance) {
                    $scope.modalOptions = tempModalOptions;
                    $scope.modalOptions.ok = function (result) {
                        $uibModalInstance.close($scope);
                    };
                    $scope.modalOptions.close = function (result) {
                        $uibModalInstance.dismiss('cancel');
                    };
                }
            }

            return $uibModal.open(tempModalDefaults).result;
        };

    }]);