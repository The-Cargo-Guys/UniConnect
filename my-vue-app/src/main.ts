import './assets/main.css';

import { createAuth0 } from "@auth0/auth0-vue";
import { createApp } from 'vue';
import 'vuetify/styles';
import { createVuetify } from 'vuetify';
import { aliases, mdi } from 'vuetify/iconsets/mdi';
import * as components from 'vuetify/components';
import * as directives from 'vuetify/directives';
import router from './router';
import App from './App.vue';

const vuetify = createVuetify({
    components,
    directives,
    theme: {
        defaultTheme: 'light'
    },
    icons: {
        defaultSet: 'mdi',
        aliases,
        sets: {
            mdi
        }
    }
});

const app = createApp(App);
app.use(router);
app.use(
    createAuth0({
        domain: import.meta.env.VITE_AUTH0_DOMAIN,
        clientId: import.meta.env.VITE_APP_AUTH0_CLIENT_ID,
        authorizationParams: {
            redirect_uri: import.meta.env.VITE_APP_AUTH0_CALLBACK,
        }
    })
)
app.use(vuetify);
app.mount('#app');
