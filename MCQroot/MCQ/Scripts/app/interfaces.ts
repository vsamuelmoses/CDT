module Mcq {
    export interface ResponseResult {
        isSucceed: boolean;
        data: any;
    }

    export interface Topic {
        id: number;
        name: string;

    }

    export interface PaymentViewModel {
        cardNumber: string;
        expiry: string;
        cvv: string;
        firstName: string;
        lastName: string;
        address: string;
        postCode: string;
        agreeWithTerms: boolean;
    }

    export interface IPaymentScope extends ng.IScope {
        paymentViewModel: PaymentViewModel;
        pay();
        validationErrors: any;
    }

    export interface TopicViewModel {
        topic: Topic;
        progress: number;
    }

    export interface RegisterViewModel {
        firstName: string;
        lastName: string;
        email: string;
        userName: string;
        password: string;
    }

    export interface IAccountInfoController extends ng.IScope {
        logOff();
        onUserLogged(user: any);
    }

    export interface ITopicsMenuScope extends ng.IScope {
        topics: TopicViewModel[];
        selectedTopic: Topic;
        selectTopic(topic: Topic);
    }


    export interface TopicSummaryViewModel {
        questionsNumber: number;
        answerdQuestionsNumber: number;
        successRate: number;
        pieData: any;
    }

    export interface DashboardViewModel {
        topicCount: number;
        questionsNumber: number;
        answerdQuestionsNumber: number;
        successRate: number;
        pieData: any;
    }

    export interface AnswerViewModel {
        id: number;
        text: string;
        selected: boolean;
        isCorrect: boolean;
    }

    export interface QuestionViewModel {
        id: number;
        name: string;
        order: number;
        answers: AnswerViewModel[];
        isFlagged: boolean;
        isAnswerCorrect?: boolean;
        userAnswers: any;
        isRevealed: boolean;
        explanation: string;
    }

    export interface ITopicSummaryScope extends ng.IScope {
        topicSummaryViewModel: TopicSummaryViewModel;
        explore();
        mockTest();
        clean();
        settings();
    }

    export interface IDashboardScope extends ng.IScope {
        dashboardViewModel: DashboardViewModel;
        explore();
        mockTest();
        clear();
        settings();
    }

    export interface IQuestionScope extends ng.IScope {
        questionViewModel: QuestionViewModel;
        questionsCache: QuestionViewModel[];
        getNextQuestionForTopic(id, p);
        getPrevQuestionForTopic(id, p);
        changeFlagState();
        selectAnswer(answer: AnswerViewModel);
        answerQuestion();
        getAnswerColor(a, q);
        state: any;
        nextButtonVisible();
        prevButtonVisible();
        slider: any;
        sliderSelectedValue: any;
        myModel: any;
        localProgress: number;
        cleanAnswers();
        isAnySelected();
        hasLikeDisplayed: boolean;
        lastAllOrder: number;
        lastUnReadedOrder: number;
        lastFlaggedOrder: number;
        totalQuestionsCountForTab: number;
        explanation();
    }

    export interface IAccountSettingsScope extends ng.IScope {
        firstName: string;
        lastName: string;
        userName: string;
        email: string;
        doesHaveAccessNow: boolean;
        lastPaymentDate: string;
        expireDate: string;
        newPassword: string;
        updatePassword();
    }

    export interface ExternalLoginViewModel extends RegisterViewModel {
        id: number;
        accessToken: string;
    }

    export interface LoginViewModel {
        email: string;
        password: string;
    }

    export interface IRegisterScope extends ng.IScope {
        registerViewModel: RegisterViewModel;
        register();
        validationErrors: any;
    }

    export interface ILoginScope extends ng.IScope {
        loginViewModel: LoginViewModel;
        login();
        loginWithFacebook();
        forgotPassword();
        validationErrors: any;
        facebookReady: boolean;
    }
} 