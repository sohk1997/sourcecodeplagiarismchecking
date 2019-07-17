Base method: 
public String checkLogin(String username, String password) throws SQLException {
    String role = "failed";
    try {
        String sql = "select r.roleName from tblUsers u, tblRoles r where u.userID = ? and u.password = ? and u.roleID = r.roleID";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        preStm.setString(1, username);
        preStm.setString(2, getEncrypPassword(password));
        rs = preStm.executeQuery();
        if (rs.next()) {
            role = rs.getString("roleName");
        }
    } catch (Exception e) {
        e.printStackTrace();
    } finally {
        closConnection();
    }
    return role;
}
Next method
public Map<String, Integer> countRole() throws SQLException {
    Map<String, Integer> map = new HashMap<>();
    try {
        String sql = "select roleID, count(roleID) as total from tblUsers group by roleID";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        rs = preStm.executeQuery();
        while (rs.next()) {
            map.put(rs.getString("roleID"), rs.getInt("total"));
        }
    } catch (Exception e) {
        e.printStackTrace();
    } finally {
        closConnection();
    }
    return map;
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
public String checkLogin(String username, String password) throws SQLException {
    String role = "failed";
    try {
        String sql = "select r.roleName from tblUsers u, tblRoles r where u.userID = ? and u.password = ? and u.roleID = r.roleID";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        preStm.setString(1, username);
        preStm.setString(2, getEncrypPassword(password));
        rs = preStm.executeQuery();
        if (rs.next()) {
            role = rs.getString("roleName");
        }
    } catch (Exception e) {
        e.printStackTrace();
    } finally {
        closConnection();
    }
    return role;
}
Next method
public String checkLogin(String username, String password) throws SQLException {
    String role = "failed";
    try {
        String sql = "select r.roleName from tblUsers u, tblRoles r where u.userID = ? and u.password = ? and u.roleID = r.roleID";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        preStm.setString(1, username);
        preStm.setString(2, getEncrypPassword(password));
        rs = preStm.executeQuery();
        if (rs.next()) {
            role = rs.getString("roleName");
        }
    } catch (Exception e) {
        e.printStackTrace();
    } finally {
        closConnection();
    }
    return role;
}
