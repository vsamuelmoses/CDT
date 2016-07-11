module Mcq {
    export module Home {
        export function dashboardController($scope: IDashboardScope, $stateParams, topicService: TopicService, $rootScope, $state, $cookies, modalService, accountService) {
            //if (!$rootScope.loggedUser)
            //    $state.go("home");
            var init = () => {
                topicService.getDashboardInfo().then((result) => {
                    if (result.isSucceed) {
                        $scope.dashboardViewModel = {
                            topicCount: result.data.topicsCount,
                            answerdQuestionsNumber: result.data.progress,
                            questionsNumber: result.data.questionsCount,
                            successRate: result.data.successRate,
                            pieData: [
                                { value: result.data.progress, color: "#7F3300" },
                                { value: 100 - result.data.progress, color: "transparent" }
                            ],
                            //myOptions :  {
                            //    percentageInnerCutout: 10,
                            //    segmentShowStroke: false
                            //}
                        };
                        $rootScope.selectedTopic = null;
                    }
                });
            }

            init();

            $scope.clear = () => {
                topicService.resetProgress().then((response) => {
                    if (response.isSucceed) {
                        for (var c in $cookies) {
                            delete $cookies[c];
                        }
                        init();
                        $rootScope.$broadcast('refreshProgress');
                    }
                });
            }

            $scope.explore = () => {
                var lastSelectedTopic = $cookies["lastVisitedTopicId"];

                var modalOptions = {
                    closeButtonText: 'Cancel',
                    actionButtonText: 'Ok',
                    headerText: '',
                    bodyText: ''
                };
                if (!$rootScope.loggedUser) {
                    modalOptions.bodyText = 'Sorry, in order to explore further, please register or login. Thanks';


                    modalService.showModal({}, modalOptions).then(function (result) {
                        $state.go("home");
                    });
                } else {
                    accountService.getPaymentInfo().then((response) => {

                        if (response.data.doesHaveAccessNow === false) {
                            modalOptions.bodyText = 'Your account has been expired. Please reactivate your account.';

                            modalService.showModal({}, modalOptions).then(function (result) {
                                $state.go("payment");
                            });
                        }
                        if (response.data.doesHaveAccessNow === true) {
                            topicService.getTopicById(lastSelectedTopic || 1).then((response) => {
                                $rootScope.selectedTopic = response.data.topics;
                                $rootScope.selectedTopic.questionCount = response.data.questionsCount;
                                $rootScope.toggleNav();
                                $state.go("quiz");
                            });
                        }
                    });
                }



               


            }
        }
    }
}

Mcq.Home.dashboardController.$inject = ["$scope", "$stateParams", "topicService", "$rootScope", "$state", "$cookies", "modalService", "accountService"];    