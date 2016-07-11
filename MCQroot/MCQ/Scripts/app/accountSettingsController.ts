module Mcq {
    export module Home {
        export function accountSettingsController($scope: IAccountSettingsScope, $rootScope, accountService: AccountService, growl) {
            

            accountService.getPaymentInfo().then((response) => {
                $scope.lastPaymentDate = response.data.lastPaymentDate;
                $scope.doesHaveAccessNow = response.data.doesHaveAccessNow;
                $scope.expireDate = response.data.expireDate;
                //if (!$scope.lastPaymentDate && $scope.doesHaveAccessNow) {

                //}
            });

            $scope.updatePassword = () => {
                accountService.updatePassword($scope.newPassword).then((response) => {
                    if (response.isSucceed) {
                        $rootScope.loggedUser.shouldSetPassword = false;
                        growl.success('Password changed.');
                    } else {
                        growl.error('Password not changed.');
                    }
                });
            }
        }
    }
}

Mcq.Home.accountSettingsController.$inject = ["$scope", "$rootScope", "accountService", "growl"];    