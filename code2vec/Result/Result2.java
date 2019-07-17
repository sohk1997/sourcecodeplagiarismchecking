Base method: 
public List<User> getAllUser() throws SQLException {
    List<User> list = new ArrayList<>();
    try {
        String sql = "select u.userID, u.firstName, u.lastName, u.email, r.roleName\n" + "from tblUsers u, tblRoles r\n" + "where u.roleID = r.roleID";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        rs = preStm.executeQuery();
        while (rs.next()) {
            String userID = rs.getString("userID");
            String firstName = rs.getString("firstName");
            String lastName = rs.getString("lastName");
            String email = rs.getString("email");
            String role = rs.getString("roleName");
            list.add(new User(userID, firstName, lastName, email, role));
        }
    } catch (Exception e) {
        e.printStackTrace();
    } finally {
        closConnection();
    }
    return list;
}
Next method
public List<Role> getAllRole() throws SQLException {
    List<Role> list = new ArrayList();
    try {
        String sql = "select roleID, roleName from tblRoles";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        rs = preStm.executeQuery();
        while (rs.next()) {
            String roleID = rs.getString("roleID");
            String roleName = rs.getString("roleName");
            list.add(new Role(roleID, roleName));
        }
    } catch (Exception e) {
        e.printStackTrace();
    } finally {
        closConnection();
    }
    return list;
}
Next method
public List<User> searchLikeName(String name) throws SQLException {
    List<User> list = new ArrayList<>();
    try {
        String sql = "select u.userID, u.firstName, u.lastName, u.email, r.roleName\n" + "from tblUsers u, tblRoles r\n" + "where u.roleID = r.roleID and \n" + "(u.firstName like ? or u.lastName like ?)";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        preStm.setString(1, "%" + name + "%");
        preStm.setString(2, "%" + name + "%");
        rs = preStm.executeQuery();
        while (rs.next()) {
            String userID = rs.getString("userID");
            String firstName = rs.getString("firstName");
            String lastName = rs.getString("lastName");
            String email = rs.getString("email");
            String role = rs.getString("roleName");
            list.add(new User(userID, firstName, lastName, email, role));
        }
    } catch (Exception e) {
        e.printStackTrace();
    } finally {
        closConnection();
    }
    return list;
}
Next method
public List<User> searchLikeName(String name) throws SQLException {
    List<User> list = new ArrayList<>();
    try {
        String sql = "select u.userID, u.firstName, u.lastName, u.email, r.roleName\n" + "from tblUsers u, tblRoles r\n" + "where u.roleID = r.roleID and \n" + "(u.firstName like ? or u.lastName like ?)";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        preStm.setString(1, "%" + name + "%");
        preStm.setString(2, "%" + name + "%");
        rs = preStm.executeQuery();
        while (rs.next()) {
            String userID = rs.getString("userID");
            String firstName = rs.getString("firstName");
            String lastName = rs.getString("lastName");
            String email = rs.getString("email");
            String role = rs.getString("roleName");
            list.add(new User(userID, firstName, lastName, email, role));
        }
    } catch (Exception e) {
        e.printStackTrace();
    } finally {
        closConnection();
    }
    return list;
}
Next method
public List<User> getAllUser() throws SQLException {
    List<User> list = new ArrayList<>();
    try {
        String sql = "select u.userID, u.firstName, u.lastName, u.email, r.roleName\n" + "from tblUsers u, tblRoles r\n" + "where u.roleID = r.roleID";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        rs = preStm.executeQuery();
        while (rs.next()) {
            String userID = rs.getString("userID");
            String firstName = rs.getString("firstName");
            String lastName = rs.getString("lastName");
            String email = rs.getString("email");
            String role = rs.getString("roleName");
            list.add(new User(userID, firstName, lastName, email, role));
        }
    } catch (Exception e) {
        e.printStackTrace();
    } finally {
        closConnection();
    }
    return list;
}
Next method
public List<User> getAllUser() throws SQLException {
    List<User> list = new ArrayList<>();
    try {
        String sql = "select u.userID, u.firstName, u.lastName, u.email, r.roleName\n" + "from tblUsers u, tblRoles r\n" + "where u.roleID = r.roleID";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        rs = preStm.executeQuery();
        while (rs.next()) {
            String userID = rs.getString("userID");
            String firstName = rs.getString("firstName");
            String lastName = rs.getString("lastName");
            String email = rs.getString("email");
            String role = rs.getString("roleName");
            list.add(new User(userID, firstName, lastName, email, role));
        }
    } catch (Exception e) {
        e.printStackTrace();
    } finally {
        closConnection();
    }
    return list;
}
