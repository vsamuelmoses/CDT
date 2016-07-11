module Mcq {
    export module Home {
        export function loginController($scope: ILoginScope, accountService: AccountService, growl, $rootScope, $state, modalService, $uibModalStack) {

            $scope.loginWithFacebook = () => {
                window["FB"].login(function (authResult) {
                    if (authResult.authResponse) {
                        window["FB"].api('/me', {
                            fields: 'last_name,first_name,email'
                        }, function (userDetailsResponse) {
                                var user = <ExternalLoginViewModel>{
                                    id: userDetailsResponse.id,
                                    lastName: userDetailsResponse.last_name,
                                    firstName: userDetailsResponse.first_name,
                                    userName: userDetailsResponse.first_name + ' ' + userDetailsResponse.last_name,
                                    email: userDetailsResponse.email,
                                    accessToken: authResult.authResponse.accessToken
                                }
                                accountService.loginExternal(user).then((response) => {
                                    if (!response.isSucceed) {
                                        growl.warning(response.data);
                                    } else {
                                        response.data.userName = response.data.firstName + " " + response.data.lastName;
                                        $rootScope.loggedUser = response.data;
                                    }
                                });

                            });
                    } else {
                        growl.warning('User cancelled login or did not fully authorize.');
                    }
                }, { scope: 'email' });
            }

            $scope.login = () => {
                if (!$scope.loginViewModel) {
                    growl.warning("Please enter login and password");
                    return;
                }
                accountService.login($scope.loginViewModel).then((response) => {
                    if (!response.isSucceed) {
                        if (response.data[0]) {
                            growl.warning(response.data[0].errors[0].errorMessage);
                        }

                        $scope.validationErrors = response.data;
                        $scope.$$phase || $scope.$apply();
                    } else {
                        response.data.userName = response.data.firstName + " " + response.data.lastName;
                        $rootScope.loggedUser = response.data;
                    }
                });
            }

            $scope.forgotPassword = () => {
                var modalOptions = {
                    closeButtonText: 'Close',
                    actionButtonText: 'Send',
                    headerText: '',
                    bodyText: '',
                    email:''
                };


                modalService.showModal({
                  
                }, modalOptions).then(function (result) {
                    accountService.resetPassword(result.modalOptions.email).then((response) => {
                        if (!response.isSucceed) {
                            growl.error(response.data);
                            return;
                        }
                        growl.success('New password has been sent');
                    });
                   
                });
            }
        }
    }
}

Mcq.Home.loginController.$inject = ["$scope", "accountService", "growl", "$rootScope", "$state", "modalService", "$uibModalStack"];   