module Mcq {
    export module Home {
        export function paymentController($scope: IPaymentScope, $stateParams, $rootScope, $state, paymentService: PaymentService, growl, $filter, modalService, $uibModalStack) {
            $scope.pay = () => {
                var modalOptions = {
                    closeButtonText: '',
                    actionButtonText: '',
                    headerText: '',
                    bodyText: 'Please wait your payment is in progress.',
                    windowClass: 'center-modal'
                };

         
                if (!$scope.paymentViewModel) {
                    growl.warning("Please enter all info");
                    return;
                }
                modalService.showModal({}, modalOptions).then(function (result) {
                });
                paymentService.pay($scope.paymentViewModel).then((response) => {
                    $uibModalStack.dismissAll();
                    if (!response.isSucceed) {
                        if (response.data[0]) {
                            growl.error(response.data);
                            $scope.validationErrors = null;
                            return;
                        }

                        $scope.validationErrors = response.data;
                        $scope.$$phase || $scope.$apply();
                    } else {
                        $state.go("dashboard");
                    }
                });
            }
           
            //$scope["open"] = function ($event) {
            //    $scope["datePickerOpened"] = true;
            //};
            //$scope["dateOptions"] = {
            //    startingDay: 1
               
            //};

            //$scope.$watch(() => { return $scope.paymentViewModel ? $scope.paymentViewModel.expiry : null; }, (n, o) => {
            //    if (!n)
            //        return;
            //    $scope.paymentViewModel.expiry = $filter('date')(n, 'MM/dd/yyyy');
            //});
        }
    }
}

Mcq.Home.paymentController.$inject = ["$scope", "$stateParams", "$rootScope", "$state", "paymentService", "growl", "$filter", "modalService", "$modalStack"];   