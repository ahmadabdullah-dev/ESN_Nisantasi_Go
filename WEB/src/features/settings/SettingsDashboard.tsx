import {Container } from "@mui/material";
import MyProfile from "../user/MyProfile";
import SettingsHeader from "./SettingsHeader";

export default function SettingsDashboard() {
  return (
    <Container maxWidth="sm" sx={{ py: 4 }}>
        <SettingsHeader />
        <MyProfile />
    </Container>
  );
}
