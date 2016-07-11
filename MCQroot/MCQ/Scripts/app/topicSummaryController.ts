module Mcq {
    export module Home {
        export function topicSummaryController($scope: ITopicSummaryScope, $stateParams, topicService: TopicService, $rootScope, $state, modalService, accountService, $cookies) {


            $scope.explore = () => {

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

                            modalService.showModal({}, modalOptions).then(function(result) {
                                $state.go("payment");
                            });
                        }
                        if (response.data.doesHaveAccessNow === true){
                            $state.go("quiz");
                        }
                    });
                }
            }
            //$state.go("quiz");
           
            //var loadTopic = (id) => {

            //}

            $scope.clean = () => {
                topicService.resetProgressForTopic($rootScope.selectedTopic.id).then((response) => {
                    if (response.isSucceed) {
                        topicService.getTopicSummary($rootScope.selectedTopic.id).then((result) => {
                            if (result.isSucceed) {

                                $rootScope.selectedTopic.questionCount = result.data.questionsCount;
                                $scope.topicSummaryViewModel = {
                                    answerdQuestionsNumber: result.data.progress,
                                    questionsNumber: result.data.questionsCount,
                                    successRate: result.data.successRate,
                                    pieData: [
                                        { value: result.data.progress, color: "#579A93" },
                                        { value: 100 - result.data.progress, color: "transparent" }
                                    ],
                                    //myOptions: {
                                    //    percentageInnerCutout: 10,
                                    //    segmentShowStroke: false
                                    //}
                                };


                            }
                        });
                        delete $cookies["all_" + $rootScope.selectedTopic.id];
                        delete $cookies["unread_" + $rootScope.selectedTopic.id];
                        delete $cookies["flagged_" + $rootScope.selectedTopic.id];
                        $rootScope.$broadcast('refreshProgress');
                    }
                });
            }

            $scope.$watch(() => {
                return $rootScope.selectedTopic;
            },(newValue, oldValue) => {
                    if (!newValue)
                        return;
                    //if (angular.equals(newValue, oldValue)) {
                    //    return;
                    //}
                    topicService.getTopicSummary(newValue.id).then((result) => {
                        if (result.isSucceed) {


                            $rootScope.selectedTopic.questionCount = result.data.questionsCount;
                            $scope.topicSummaryViewModel = {
                                answerdQuestionsNumber: result.data.progress,
                                questionsNumber: result.data.questionsCount,
                                successRate: result.data.successRate,
                                pieData: [
                                    { value: result.data.progress, color: "#7F3300" },
                                    { value: 100 - result.data.progress, color: "transparent" }
                                ],
                                //myOptions: {
                                //    percentageInnerCutout: 10,
                                //    segmentShowStroke: false
                                //}
                            };


                        }
                    });
                });


        }
    }
}

Mcq.Home.topicSummaryController.$inject = ["$scope", "$stateParams", "topicService", "$rootScope", "$state", "modalService", "accountService", "$cookies"];  