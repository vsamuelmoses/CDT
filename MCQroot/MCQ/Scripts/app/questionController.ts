module Mcq {
    export module Quiz {
        export function questionController($scope: IQuestionScope, $stateParams, $rootScope, $state, quizService: QuizService, growl, modalService, $cookies) {
            if (!$rootScope.selectedTopic)
                return;
            var modalOptions = {
                closeButtonText: 'Close',
                actionButtonText: '',
                headerText: 'If you like our app, please like us on Facebook',
                windowClass: 'center-modal'
            };
            ;

            $scope.localProgress = 0;
            $scope.$watch("localProgress",(n, o) => {
                if (n === o)
                    return;
                var percentage = (n / $rootScope.selectedTopic.questionCount) * 100;
                if (percentage > 2 && !$cookies["hasLiked"]) {
                    modalService.showModal({}, modalOptions).then(function (result) {
                    });
                    $cookies["hasLiked"] = 'true';
                }
            });
            $scope.state = 0;
            $scope.questionsCache = [];
            var topicSwitchHandler = (methodName: string, order: number, diff: number, p, s) => {
                $scope.state = p;
                //if (p === 0) {
                //    var q = Enumerable.from($scope.questionsCache).firstOrDefault((x) => { return x.order === order + diff; });
                //    if (q && $scope.state === p) {
                //        q.isFlagged = Enumerable.from<number>($rootScope.flaggedQuestionsIds).any((x) => { return x === q.id });
                //        $scope.questionViewModel = q;
                //        $scope.sliderSelectedValue = q.order;
                //        $scope.$$phase || $scope.$apply();
                //        return;
                //    }
                //}

                //p === 0 ? order : $scope.questionViewModel.id
                quizService[methodName]($rootScope.selectedTopic.id, order, p, s).then((response) => {
                    if (response.data == null) {
                        return;
                    }
                    var vm = {
                        id: response.data.question.id,
                        name: response.data.question.name,
                        //order: p === 0 ? response.data.question.order : response.data.question.id,
                        order: response.data.question.order,
                        answers: response.data.question.answers,
                        isFlagged: false,
                        isAnswerCorrect: response.data.isCorrect,
                        userAnswers: response.data.userAnswers,
                        isRevealed: false,
                        explanation: response.data.question.explenation,
                        imageUrl: response.data.question.imageUrl
                    }
                    //Enumerable.from(vm.userAnswers).forEach((e, i) => {
                    //    Enumerable.from<any>(vm.answers).first((x) => { return x.id === e.id }).selected = true;
                    //});
                    vm.isFlagged = Enumerable.from($rootScope.flaggedQuestionsIds).any((x) => { return x === vm.id });
                    $scope.questionsCache.push(angular.copy(vm));
                    $scope.questionViewModel = vm;
                    if (p === 0) {
                        $scope.sliderSelectedValue = vm.order;
                    }
                    if (p === 2) {
                        $scope.sliderSelectedValue = $rootScope.flaggedQuestionsIds.indexOf(vm.id) + 1;
                    }
                    if (p === 1) {
                        $scope.sliderSelectedValue = $rootScope.unReadedQuestions.indexOf(vm.id) + 1;
                    }
                    if (vm.isAnswerCorrect) {
                        $scope.localProgress++;
                    }

                    $cookies[$rootScope.selectedTopic.id + "_last"] = vm.order;
                    $cookies["lastVisitedTopicId"] = $rootScope.selectedTopic.id;
                    if (p === 0) {
                        //$rootScope.lastAllOrder = order;
                        $cookies["all_" + $rootScope.selectedTopic.id] = order;

                    }
                    if (p === 1) {
                        $rootScope.lastUnreadedOrder = order;
                        $cookies["unread_" + $rootScope.selectedTopic.id] = order;

                    }
                    if (p === 2) {
                        $rootScope.lastFlaggedOrder = order;
                        $cookies["flagged_" + $rootScope.selectedTopic.id] = order;
                    }

                });
            }

            $scope.getAnswerColor = (a: AnswerViewModel, q: QuestionViewModel) => {
                if (!$scope.questionViewModel.isRevealed && !a.selected) {
                    return '';
                }
                if (!$scope.questionViewModel.isRevealed && a.selected) {
                    return 'active';
                }
                if ($scope.questionViewModel.isRevealed === true && a.isCorrect === true && a.selected === true) {
                    return 'active correct';
                }
                if ($scope.questionViewModel.isRevealed === true && !a.isCorrect && a.selected === true) {
                    return 'active incorrect';
                }

                if ($scope.questionViewModel.isRevealed === true && a.isCorrect && !a.selected) {
                    return 'correct';
                }
                if ($scope.questionViewModel.isRevealed === true && !a.isCorrect && !a.selected) {
                    return 'incorrect';
                }

                //if (q.isAnswerCorrect === null && !a.selected)
                //    return '';
                //if (q.isAnswerCorrect === null && a.selected)
                //    return 'selected-answer';
                //if (a.selected && a.isCorrect)
                //    return 'selected-answer';
                //if (!a.selected && a.isCorrect)
                //    return 'answer-correct-not-selected';
                //if (a.selected && !a.isCorrect)
                //    return 'answer-not-correct-selected';
                //return '';

            }

            $scope.getNextQuestionForTopic = (order, p) => {
                topicSwitchHandler("getNextQuestionForTopic", order, 1, p === undefined ? $scope.state : p, false);
            }

            $scope.getPrevQuestionForTopic = (order, p) => {
                topicSwitchHandler("getPrevQuestionForTopic", order, -1, p === undefined ? $scope.state : p, false);
            }

            $scope.nextButtonVisible = () => {
                if ($scope.state === 2) {
                    return $rootScope.flaggedQuestionsIds[$rootScope.flaggedQuestionsIds.length - 1] !== $scope.questionViewModel.id;
                }
                if ($scope.state === 1) {
                    return $rootScope.unReadedQuestions[$rootScope.unReadedQuestions.length - 1] !== $scope.questionViewModel.id;
                }
                if (!$scope.questionViewModel)
                    return false;
                return $scope.questionViewModel.order < $rootScope.selectedTopic.questionCount;
            }

            $scope.prevButtonVisible = () => {
                if ($scope.state === 2) {
                    return $rootScope.flaggedQuestionsIds[0] !== $scope.questionViewModel.id;
                }
                if ($scope.state === 1) {
                    return $rootScope.unReadedQuestions[0] !== $scope.questionViewModel.id;
                }
                if (!$scope.questionViewModel)
                    return false;
                return $scope.questionViewModel.order > 1;
            }

            $scope.changeFlagState = () => {
                quizService.changeFlagState($scope.questionViewModel.id).then((data) => {
                    if (data.isSucceed) {
                        $scope.questionViewModel.isFlagged = !$scope.questionViewModel.isFlagged;
                        if ($scope.questionViewModel.isFlagged) {
                            $rootScope.flaggedQuestionsIds.push($scope.questionViewModel.id);

                        } else {
                            $rootScope.flaggedQuestionsIds.splice(Enumerable.from<number>($rootScope.flaggedQuestionsIds).indexOf((x) => { return x === $scope.questionViewModel.id }), 1);
                            $cookies["flagged_" + $rootScope.selectedTopic.id] = 0;
                        }
                        if ($scope.state === 2) {
                            init($scope.state);
                        }
                    }
                });
            }

            $scope.selectAnswer = (answer: AnswerViewModel) => {
                if ($scope.questionViewModel.isRevealed === true) {
                    $scope.cleanAnswers();
                }
                answer.selected = !answer.selected;
            };

            $scope.isAnySelected = () => {
                if (!$scope.questionViewModel)
                    return true;
                return Enumerable.from($scope.questionViewModel.answers).any(x => x.selected);
            }

            $scope.answerQuestion = () => {
                if (!Enumerable.from($scope.questionViewModel.answers).any((a) => { return a.selected })) {
                    //growl.warning('Please select at least one answer.');
                    return;
                }

                quizService.revealAnswer({
                    questionId: $scope.questionViewModel.id,
                    answers: Enumerable.from($scope.questionViewModel.answers).where((a) => { return a.selected; }).select((a) => { return a.id; }).toArray()
                }).then((response) => {
                    $scope.questionViewModel.isAnswerCorrect = response.isSucceed;
                    $rootScope.$broadcast('refreshProgress');
                    $rootScope.$broadcast("initTabsData");
                    $scope.questionViewModel.isRevealed = true;

                });
            }

            $scope.cleanAnswers = () => {
                Enumerable.from($scope.questionViewModel.answers).forEach((x) => {
                    x.selected = false;

                });
                $scope.questionViewModel.isAnswerCorrect = null;
                $scope.questionViewModel.isRevealed = false;
            }

            var init = (p) => {
                var lastId = 0;
                if (!$cookies["all_" + $rootScope.selectedTopic.id] && p === 0) {
                    lastId = $cookies[$rootScope.selectedTopic.id + "_last"] - 1;
                    $scope.totalQuestionsCountForTab = $rootScope.selectedTopic.questionCount;
                } else {
                    if (p === 0) {
                        //lastId = $rootScope.lastAllOrder;
                        lastId = $cookies["all_" + $rootScope.selectedTopic.id];
                        $scope.totalQuestionsCountForTab = $rootScope.selectedTopic.questionCount;
                    }
                    if (p === 1) {
                        //lastId = $rootScope.lastUnreadedOrder;
                        lastId = $cookies["unread_" + $rootScope.selectedTopic.id];
                        $scope.totalQuestionsCountForTab = $rootScope.unReadedQuestions.length;
                    }
                    if (p === 2) {
                        //lastId = $rootScope.lastFlaggedOrder;
                        lastId = $cookies["flagged_" + $rootScope.selectedTopic.id];
                        $scope.totalQuestionsCountForTab = $rootScope.flaggedQuestionsIds.length;
                    }
                }

                $scope.getNextQuestionForTopic(lastId || 0, p || 0);
                if ($scope.state === 2 && $rootScope.flaggedQuestionsIds.length === 0) {
                    init(0);
                    $rootScope.$broadcast('changeTab');
                }
            }

            $scope.$on("initQuestion",(e, k) => {

                init(k);
            });

            $scope.explanation = () => {
                modalService.showModal({
                    templateUrl: 'Explanation'
                }, {
                        bodyText: $scope.questionViewModel.explanation,
                        closeButtonText: 'Close',
                        actionButtonText: '',
                        headerText: 'Answer Explanation',
                        windowClass: 'center-modal'
                    }).then(function (result) {

                });
            }

            $scope.slider = {
                'options': {
                    start: function (event, ui) { },
                    stop: function (event, ui) {
                        var sliderValue = ui.value - 1;
                        if ($scope.state === 1) {
                            sliderValue = $rootScope.unReadedQuestions[ui.value - 1];
                        }
                        if ($scope.state === 2) {
                            sliderValue = $rootScope.flaggedQuestionsIds[ui.value - 1];
                        }
                        topicSwitchHandler("getNextQuestionForTopic", sliderValue, 1, $scope.state, true);
                    }
                }
            }
        };



    }
}

Mcq.Quiz.questionController.$inject = ["$scope", "$stateParams", "$rootScope", "$state", "quizService", "growl", "modalService", "$cookies"];   