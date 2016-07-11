module Mcq {
    export module Home {
        export function homeController($scope, $stateParams, $rootScope, $state) {
            if ($rootScope.loggedUser) {
                $state.go("dashboard");
            }

            
        }
    }
}

Mcq.Home.homeController.$inject = ["$scope", "$stateParams", "$rootScope", "$state"]; 