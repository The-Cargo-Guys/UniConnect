<template>
    <div id="societies-page" class="container">
      <h1>Societies</h1>
      <button class="add-society" @click="goToAddSociety">+ Add Society</button>
      <div class="societies-list">
        <div
          v-for="society in societies"
          :key="society.id"
          class="society-card"
          @click="openSociety(society.id)"
        >
          <img :src="society.banner" alt="Society Banner" class="society-banner" />
          <div class="society-info">
            <h2>{{ society.name }}</h2>
            <p>{{ society.description }}</p>
            <!-- The join button stops propagation so it doesn't trigger navigation -->
            <button @click.stop="joinSociety(society.id)" class="join-btn">
              Join
            </button>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent, ref } from "vue";
  import { useRouter } from "vue-router";
  
  export default defineComponent({
    name: "SocietiesPage",
    setup() {
      const router = useRouter();
      const societies = ref([
        {
          id: 1,
          name: "Tech Enthusiasts",
          description: "A society for tech lovers to discuss and share knowledge.",
          banner: "https://via.placeholder.com/600x200",
        },
        {
          id: 2,
          name: "Gaming Club",
          description: "Join us for weekly gaming sessions and tournaments!",
          banner: "./images/UniConnect.png",
        },
      ]);
  
      const joinSociety = (id: number) => {
        alert(`Joined society with ID: ${id}`);
      };
  
      const goToAddSociety = () => {
        console.log("Routes:", router.getRoutes());
        router.push("/add-society").catch((err) => {
          console.error("Navigation to /add-society failed:", err);
        });
      };
  
      const openSociety = (id: number) => {
        console.log("Opening society with id:", id);
        router.push(`/societies/${id}`).catch((err) => {
            console.error("Navigation failed:", err);
        });
    };

      return {
        societies,
        joinSociety,
        goToAddSociety,
        openSociety,
      };
    },
  });
  </script>
  
  <style scoped>
  .container {
    max-width: 1200px;
    margin: auto;
    padding: 2rem;
    text-align: center;
  }
  
  h1 {
    font-size: 2rem;
    margin-bottom: 1rem;
  }
  
  .add-society {
    background-color: #4a90e2;
    color: white;
    padding: 10px 20px;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    margin-bottom: 1rem;
  }
  
  .societies-list {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
    gap: 20px;
  }
  
  .society-card {
    background: #d1caca;
    padding: 1rem;
    border-radius: 10px;
    text-align: left;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
    cursor: pointer;
    transition: transform 0.2s;
  }
  
  .society-card:hover {
    transform: scale(1.02);
  }
  
  .society-banner {
    width: 100%;
    height: 150px;
    object-fit: cover;
    border-radius: 8px;
  }
  
  .society-info {
    padding: 10px 0;
  }
  
  .join-btn {
    background-color: #ff4081;
    color: white;
    padding: 8px 16px;
    border: none;
    border-radius: 5px;
    cursor: pointer;
  }
  </style>
  