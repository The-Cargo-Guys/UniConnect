import TheCommunitiesPage from './TheCommunitiesPage.vue';
import TheHomePage from './TheHomePage.vue';
import TheSocietiesPage from './TheSocietiesPage.vue';
import TheAddSocietyPage from './TheAddSocietyPage.vue';
import TheRegistryPage from './TheRegistryPage.vue';
import TheCallbackPage from "./TheCallbackPage.vue";
import SocietyDetailsPage from './SocietyDetailsPage.vue';
import { createRouter as createVueRouter, createWebHistory } from 'vue-router';

const routes = [
    { path: "/", redirect: "/home" },
    { path: "/registry", component: TheRegistryPage },
    { path: "/home", component: TheHomePage },
    { path: "/societies", component: TheSocietiesPage },
    { path: "/add-society", component: AddSocietyPage },
    { path: "/societies/:id", name: "SocietyDetails", component: SocietyDetailsPage },
];

const router = createVueRouter({
    history: createWebHistory(),
    routes,
});

export { TheCommunitiesPage, TheHomePage, TheSocietiesPage, TheRegistryPage, AddSocietyPage, TheCallbackPage, SocietyDetailsPage };
export default router;
