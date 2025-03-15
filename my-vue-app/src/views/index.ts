import TheCommunitiesPage from './TheCommunitiesPage.vue';
import TheHomePage from './TheHomePage.vue';
import TheSocietiesPage from './TheSocietiesPage.vue';
import AddSocietyPage from './AddSocietyPage.vue';
import TheLoginPage from './TheLoginPage.vue';
import TheRegistryPage from './TheRegistryPage.vue';
import TheSignupPage from "./TheSignupPage.vue";
import TheCallbackPage from "./TheCallbackPage.vue";
import { createRouter as createVueRouter, createWebHistory } from 'vue-router';

const routes = [
    { path: "/", redirect: "/home" },
    { path: "/login", component: TheLoginPage },
    { path: "/signup", component: TheSignupPage },
    { path: "/registry", component: TheRegistryPage },
    { path: "/home", component: TheHomePage },
    { path: "/societies", component: TheSocietiesPage },
    { path: "/add-society", component: AddSocietyPage },
];

const router = createVueRouter({
    history: createWebHistory(),
    routes,
});

export { TheCommunitiesPage, TheHomePage, TheSocietiesPage, TheLoginPage, TheRegistryPage, TheSignupPage, AddSocietyPage, TheCallbackPage };
export default router;